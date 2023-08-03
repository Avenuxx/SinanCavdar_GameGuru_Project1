using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareController : MonoBehaviour
{
    GameManager manager;
    UIManager uiManager;

    public List<SquareController> neighbors;

    public GameObject crossObject;

    public int xPosition;
    public int yPosition;

    public bool _isChosen;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    void Start()
    {
        CheckForNeighbors();
    }

    private void CheckForNeighbors()
    {
        List<GameObject> currentSquaresList = manager.lists.currentSquares;
        foreach (GameObject square in currentSquaresList)
        {
            SquareController squareController = square.GetComponent<SquareController>();
            if (PositionCheck(squareController, -1, 0) || PositionCheck(squareController, 0, -1)
                || PositionCheck(squareController, 1, 0) || PositionCheck(squareController, 0, 1))
            {
                neighbors.Add(squareController);
            }
        }
    }
    private bool PositionCheck(SquareController squareController, int plusPosX, int plusPosY)
    {
        return squareController.xPosition == xPosition + plusPosX && squareController.yPosition == yPosition + plusPosY;
    }

    void Update()
    {

    }

    private void OnMouseDown()
    {
        _isChosen = true;
        crossObject.GetComponent<Animator>().SetBool("isChosen", true);
        EventManager.Broadcast(GameEvent.OnCheckForCrossCombo);
        EventManager.Broadcast(GameEvent.OnPlaySound, "ButtonClick");
        OnCrossListRenew();
    }

    public void OnCheckForCrossCombo()
    {
        int neighborCrossCount = 0;
        foreach (SquareController neighbor in neighbors)
        {
            if (neighbor._isChosen)
            {
                neighborCrossCount++;
            }
        }

        if (neighborCrossCount < 2 || !_isChosen)
            return;

        foreach (SquareController neighbor in neighbors)
        {
            if (neighbor._isChosen && !manager.lists.willBeDestroy.Contains(neighbor.gameObject))
            {
                manager.lists.willBeDestroy.Add(neighbor.gameObject);
            }
        }

        if (!manager.lists.willBeDestroy.Contains(gameObject))
        {
            manager.lists.willBeDestroy.Add(gameObject);
        }
    }

    private void OnCrossListRenew()
    {
        List<GameObject> destroyList = manager.lists.willBeDestroy;
        if (destroyList.Count == 0)
            return;

        foreach (GameObject destroySquare in destroyList)
        {
            SquareController squareController = destroySquare.GetComponent<SquareController>();
            squareController.crossObject.GetComponent<Animator>().SetBool("isChosen", false);
            squareController._isChosen = false;
        }
        destroyList.Clear();

        manager.ints.matchCount++;
        uiManager.texts.matchCountText.text = "Match Count: " + manager.ints.matchCount;
        EventManager.Broadcast(GameEvent.OnPlaySound, "Pop");
    }

    ////////////////////////////// EVENTS ////////////////////////////
    public void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCheckForCrossCombo, OnCheckForCrossCombo);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCheckForCrossCombo, OnCheckForCrossCombo);
    }
}
