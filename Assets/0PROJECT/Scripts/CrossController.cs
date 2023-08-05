using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossController : MonoBehaviour
{
    SquareController squareController;
    GameManager manager;
    UIManager uiManager;

    private void Awake()
    {
        squareController = GetComponent<SquareController>();
        manager = squareController.manager;
        uiManager = squareController.uiManager;
    }

    public void OnCheckForCrossCombo()
    {
        //CHECK FOR EVERY NEIGHBOR TO KNOW HOW MANY OF THEM GOT CROSS
        int neighborCrossCount = 0;
        foreach (SquareController neighbor in squareController.neighbors)
        {
            if (neighbor._isChosen)
            {
                neighborCrossCount++;
            }
        }

        //CHECK FOR CROSS COUNT THAT 2 NEIGHBORS HAVE CROSS OR NOT
        if (neighborCrossCount < 2 || !squareController._isChosen)
            return;

        EventManager.Broadcast(GameEvent.OnAddForDestroy, this.gameObject);
    }

    private void OnCrossListRenew(object value)
    {
        //SET DESTROY SQUARE LIST
        List<GameObject> destroyList = manager.lists.willBeDestroy;
        if (destroyList.Count == 0 || (GameObject)value != this.gameObject)
            return;

        //DESTROY SQUARES GOT TRIPLE CROSS
        foreach (GameObject destroySquare in destroyList)
        {
            SquareController squareController = destroySquare.GetComponent<SquareController>();
            squareController.crossObject.GetComponent<Animator>().SetBool("isChosen", false);
            squareController._isChosen = false;
        }
        destroyList.Clear();

        //EARN SCORE AND SET SCORE TEXT
        manager.ints.matchCount++;
        uiManager.texts.matchCountText.text = "Match Count: " + manager.ints.matchCount;
        EventManager.Broadcast(GameEvent.OnPlaySound, "Pop");
    }

    public void OnAddForDestroy(object value)
    {
        if ((GameObject)value != this.gameObject)
            return;

        //ADD SQUARES WITH CROSS TO DESTROY LIST
        foreach (SquareController neighbor in squareController.neighbors)
        {
            if (neighbor._isChosen && !manager.lists.willBeDestroy.Contains(neighbor.gameObject))
            {
                manager.lists.willBeDestroy.Add(neighbor.gameObject);
            }
        }

        //IF LIST DOESNT CONTAINS GAMEOBJECT ADD ALSO THIS GAMEOBJECT
        if (!manager.lists.willBeDestroy.Contains(gameObject))
        {
            manager.lists.willBeDestroy.Add(gameObject);
        }
    }


    ////////////////////////////// EVENTS ////////////////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCheckForCrossCombo, OnCheckForCrossCombo);
        EventManager.AddHandler(GameEvent.OnCrossListRenew, OnCrossListRenew);
        EventManager.AddHandler(GameEvent.OnAddForDestroy, OnAddForDestroy);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCheckForCrossCombo, OnCheckForCrossCombo);
        EventManager.RemoveHandler(GameEvent.OnCrossListRenew, OnCrossListRenew);
        EventManager.RemoveHandler(GameEvent.OnAddForDestroy, OnAddForDestroy);
    }
}
