using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class SlotSymbol : MonoBehaviour
{
    public int Index;
    public int[] Payout;

    public Sprite Sprite
    {
        get { return _sprite; }
        set
        {
            GetComponent<SpriteRenderer>().sprite = value;
            _sprite = value;
        }
    }
    private Sprite _sprite;
}
