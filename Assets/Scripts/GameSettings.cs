using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class for storing extra setting like players names and master volume value, as well as hits to win, 
//but, unfortunatelly haven't finish it yet, due to unfortunate personal situations.
public class GameSettings : MonoBehaviour
{
    private static GameSettings instance;
    private AudioManager audioManager;

    /// <summary>
    /// Name that could be entered for player one
    /// </summary>
    private string nameOfPlayerOne;
    public string NameOfPlayerOne { get => nameOfPlayerOne; set => nameOfPlayerOne = value; }

    /// <summary>
    /// Name that could be entered for player two
    /// </summary>
    private string nameOfPlayerTwo;
    public string NameOfPlayerTwo { get => nameOfPlayerTwo; set => nameOfPlayerTwo = value; }

    /// <summary>
    /// Control for master volume, could be available in options screen
    /// </summary>
    private float _masterVolume;
    public float MasterVolume { get => _masterVolume; set => _masterVolume = value; }

    /// <summary>
    /// Variable for entering number of hits necessary to win, realised myb like dropdown list
    /// </summary>
    private int _firstTo;
    public int FirstTo { get => _firstTo; set => _firstTo = value; }

    /// <summary>
    /// Implementing singleton
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Getting only audioManager in game, for masterVolumeChange functionality
    /// </summary>
    private void Start()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();

        FirstTo = 3;
    }
}
