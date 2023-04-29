using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Data/New Game Config", order = 0)]
public class GameConfig : ScriptableObject
{
    [Header("Slot Machine Parameters")]
    public ReelConfig[] Reels;
    public LineConfig[] PayoutLines;
    [Range(3f, 10f)]
    public float SpinTime = 5f;
    [Range(10f, 40f)]
    public float SpinSpeed = 20f;

    public int ROWS = 3;
}