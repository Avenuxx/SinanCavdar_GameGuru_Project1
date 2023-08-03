using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/GameData", order = 1)]

public class GameData : ScriptableObject
{
    public void FullSource()
    {
        
    }

    void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}

