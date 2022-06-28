using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    public int boardSize = 5;

    public Leader[] leaders;

    [Header("Game Sounds")]
    public AudioClip legalMove;
    public AudioClip completeBox;

    public GameData()
    {

    }
}

[Serializable]
public class Leader
{
    public string lead;
    public int wins;
}
