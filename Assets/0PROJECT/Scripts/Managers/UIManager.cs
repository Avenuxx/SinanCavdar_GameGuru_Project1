using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Serializable]
    public struct Texts
    {
        public TMP_InputField inputText;
    }

    public Texts texts;

   public void ReGenerate()
    {
        EventManager.Broadcast(GameEvent.OnGenerateGrid);
    }
}
