// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioPlay;
    public AudioSource soundPlay;
    float timer;

    private void Awake()
    {
        // DontDestroyOnLoad(this.gameObject);

    }

    void Update()
    {

    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlaySound, OnPlaySound);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlaySound, OnPlaySound);
    }


    private void OnPlaySound(object value)
    {
        audioPlay.clip = Resources.Load<AudioClip>((string)value);
        audioPlay.PlayOneShot(audioPlay.clip);
    }
}
