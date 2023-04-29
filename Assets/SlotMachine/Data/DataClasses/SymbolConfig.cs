using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SymbolConfig", menuName = "Data/New Symbol Config", order = 2)]
public class SymbolConfig : ScriptableObject
{
    public int Index;
    public int[] Payout = new int[5] { 0, 0, 20, 50, 100 };
    public Sprite Sprite;
}