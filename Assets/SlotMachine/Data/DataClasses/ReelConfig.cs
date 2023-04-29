using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReelConfig", menuName = "Data/New Reel Config", order = 1)]
public class ReelConfig : ScriptableObject
{
    public SymbolConfig[] SymbolConfigs;
}
