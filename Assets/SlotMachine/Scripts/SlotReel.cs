using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlotReel : MonoBehaviour
{
    public List<SlotSymbol> SlotSymbols = new List<SlotSymbol>();
    public List<SlotSymbol> VisibleSymbols = new List<SlotSymbol>();
    public UnityAction OnReelFinalized;

    private float _spinSpeed;

    private bool _isSpinning = false;
    private bool _isFinalized = true;

    private void FixedUpdate()
    {
        if (_isSpinning)
        {
            transform.position += Vector3.up * _spinSpeed * Time.deltaTime;

            if (transform.position.y >= (SlotSymbols.Count - GameManager.Instance.GameConfig.Reels.Length) * SlotSymbols[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y)
            {
                transform.position = new Vector3(transform.position.x, 0);
            }
        }

        else
        {
            //Snap reel symbols to viewport
            float SnapReel(float pos)
            {
                return pos - (pos % GameManager.Instance.GetSpriteGridSize());
            }

            float reelPosOffset = Mathf.Lerp(transform.position.y, SnapReel(transform.position.y), 0.3f);
            transform.position = new Vector3(transform.position.x, reelPosOffset);

            //Wait until reel symbols are snapped
            float IsAligned(float pos)
            {
                return pos % GameManager.Instance.GetSpriteGridSize();
            }

            if (IsAligned(transform.position.y) >= 0.1f)
                return;

            if (_isFinalized)
                return;
            
            //Get snapped visible symbols for payout computation
            FinalizeSymbols();
        }
    }

    void FinalizeSymbols()
    {
        if (transform.localPosition.y <= 0.1f)
        {
            for (int i = 0; i < GameManager.Instance.GameConfig.ROWS; i++)
            {
                VisibleSymbols.Add(SlotSymbols[i]);
            }
        }
        else
        {
            for (int i = 0; i < SlotSymbols.Count; i++)
            {
                if (transform.localPosition.y - Mathf.Abs(SlotSymbols[i].transform.localPosition.y) < 1f)
                {
                    for (int j = 0; j < GameManager.Instance.GameConfig.ROWS; j++)
                    {
                        int index = (i + j >= SlotSymbols.Count) ? i + j - SlotSymbols.Count : i + j;
                        VisibleSymbols.Add(SlotSymbols[index]);
                    }
                    break;
                }
            }
        }

        _isFinalized = true;
        OnReelFinalized?.Invoke();
    }

    public void StartSpin(float speed)
    {
        _spinSpeed = speed;
        _isSpinning = true;
        _isFinalized = false;
        VisibleSymbols.Clear();
    }

    public void StopSpin()
    {
        _isSpinning = false;
    }
}
