using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareController : MonoBehaviour
{
    [HideInInspector] public GameManager manager;
    [HideInInspector] public UIManager uiManager;

    [Header("Lists")]
    public List<SquareController> neighbors;

    [Space(10)]
    [Header("GameObjects")]
    public GameObject crossObject;

    [Space(10)]
    [Header("Ints & Floats")]
    public int xPosition;
    public int yPosition;

    [Space(10)]
    [Header("Bools")]
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
        //ADD NEIGHBORS TO LIST
        List<GameObject> currentSquaresList = manager.lists.currentSquares;
        foreach (GameObject square in currentSquaresList)
        {
            SquareController squareController = square.GetComponent<SquareController>();

            //CHECK FOR EVERY SIDE OF THE SQUARE
            if (PositionCheck(squareController, -1, 0) || PositionCheck(squareController, 0, -1)
                || PositionCheck(squareController, 1, 0) || PositionCheck(squareController, 0, 1))
            {
                neighbors.Add(squareController);
            }
        }
    }

    //CHECK NEIGHBORS POSITIONS FOR LIST
    private bool PositionCheck(SquareController squareController, int plusPosX, int plusPosY)
    {
        return squareController.xPosition == xPosition + plusPosX && squareController.yPosition == yPosition + plusPosY;
    }

    private void OnMouseDown()
    {
        _isChosen = true;
        crossObject.GetComponent<Animator>().SetBool("isChosen", true);
        EventManager.Broadcast(GameEvent.OnCheckForCrossCombo);
        EventManager.Broadcast(GameEvent.OnPlaySound, "ButtonClick");
        EventManager.Broadcast(GameEvent.OnCrossListRenew, this.gameObject);
    }
}
