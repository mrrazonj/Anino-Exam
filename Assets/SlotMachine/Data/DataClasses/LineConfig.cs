using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LineConfig", menuName = "Data/New Line Config", order = 3)]
public class LineConfig : ScriptableObject
{
    [Range(0, 2)]
    public int[] VisibleRow;
}

