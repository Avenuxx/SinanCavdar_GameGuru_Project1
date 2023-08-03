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

    void CheckForNeighbors()
    {
        List<GameObject> currentSquaresList = manager.lists.currentSquares;
        int currentSquaresCount = manager.lists.currentSquares.Count;
        for (int i = 0; i < currentSquaresCount; i++)
        {
            SquareController squareController = currentSquaresList[i].GetComponent<SquareController>();
            PositionCheck(squareController, -1, 0);
            PositionCheck(squareController, 0, -1);
            PositionCheck(squareController, 1, 0);
            PositionCheck(squareController, 0, 1);
        }
    }

    void PositionCheck(SquareController squareController, int plusPosX, int plusPosY)
    {
        if (squareController.xPosition == this.xPosition + plusPosX && squareController.yPosition == this.yPosition + plusPosY)
            neighbors.Add(squareController);
    }

    void Update()
    {

    }

    private void OnMouseDown()
    {
        _isChosen = true;
        crossObject.GetComponent<Animator>().SetBool("isChosen", true);
        EventManager.Broadcast(GameEvent.OnCheckForCrossCombo);
        OnCrossListRenew();
    }

    public void OnCheckForCrossCombo()
    {
        int listCount = neighbors.Count;
        int neighborCrossCount = 0;
        for (int i = 0; i < listCount; i++)
        {
            if (neighbors[i].GetComponent<SquareController>()._isChosen)
            {
                neighborCrossCount++;
            }
        }

        if (neighborCrossCount < 2 || !_isChosen)
            return;

        for (int i = 0; i < listCount; i++)
        {
            if (neighbors[i].GetComponent<SquareController>()._isChosen && !manager.lists.willBeDestroy.Contains(neighbors[i].gameObject))
            {
                manager.lists.willBeDestroy.Add(neighbors[i].gameObject);
            }
        }

        if (!manager.lists.willBeDestroy.Contains(gameObject))
        {
            manager.lists.willBeDestroy.Add(gameObject);
        }
    }

    void OnCrossListRenew()
    {
        List<GameObject> destroyList = manager.lists.willBeDestroy;
        int listCount = destroyList.Count;

        if (listCount == 0)
            return;

        for (int i = 0; i < listCount; i++)
        {
            SquareController squareController = destroyList[i].GetComponent<SquareController>();
            squareController.crossObject.GetComponent<Animator>().SetBool("isChosen", false);
            squareController._isChosen = false;
        }
        destroyList.Clear();

        manager.ints.matchCount++;
        uiManager.texts.matchCountText.text = "Match Count: " + manager.ints.matchCount;
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
