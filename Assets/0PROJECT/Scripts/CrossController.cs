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
        int neighborCrossCount = 0;
        foreach (SquareController neighbor in squareController.neighbors)
        {
            if (neighbor._isChosen)
            {
                neighborCrossCount++;
            }
        }

        if (neighborCrossCount < 2 || !squareController._isChosen)
            return;

        EventManager.Broadcast(GameEvent.OnAddForDestroy, this.gameObject);
    }

    private void OnCrossListRenew(object value)
    {
        List<GameObject> destroyList = manager.lists.willBeDestroy;
        if (destroyList.Count == 0 || (GameObject)value != this.gameObject)
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

    public void OnAddForDestroy(object value)
    {
        if ((GameObject)value != this.gameObject)
            return;

        foreach (SquareController neighbor in squareController.neighbors)
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
