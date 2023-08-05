using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public struct Lists
    {
        public List<GameObject> currentSquares;
        public List<GameObject> willBeDestroy;
    }

    [Serializable]
    public struct Ints
    {
        public int matchCount;
    }

    public Lists lists;
    public Ints ints;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            //GENERATE GRID EVENT
            EventManager.Broadcast(GameEvent.OnGenerateGrid);
        }
    }
}
