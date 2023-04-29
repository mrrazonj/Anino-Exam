using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] OverlayUI _overlayUI;

    private List<SlotReel> _slotReels = new List<SlotReel>();
    private GameConfig _gameConfig;
    private GameManager _gameManager;

    public int ReelsFinalized = 0;

    private int _maxBet = 0;
    private int _playerBet = 0;
    private int _totalBet = 0;

    private bool _isSpinning = false;

    #region INITIALIZATION
    private void InitializeCamera()
    {
        //Set up camera orthographic size and pitch depending on number of reels
        Camera mainCamera = Camera.main;
        float spriteSize = _gameManager.GetSpriteGridSize();

        //Computation for orthographics size based on sprite size and number of reels relative to the screen dimensions
        mainCamera.orthographicSize = (spriteSize * (GameManager.Instance.GameConfig.Reels.Length + 2)) * Screen.height / Screen.width * 0.5f;

        //Set camera position at the mid point of the reels and keeping the top behind the canvas mask
        mainCamera.transform.position = (transform.GetChild(0).position + transform.GetChild(transform.childCount - 1).position) / 2;
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, -0.52f * GameManager.Instance.GameConfig.Reels.Length);
    }

    private void InitializeSlotMachine()
    {
        //Spawn Reels
        for (int i = 0; i < _gameConfig.Reels.Length; i++)
        {
            GameObject reelObject = new GameObject($"Reel{i+1}");
            SlotReel slotReel = reelObject.AddComponent<SlotReel>();
            slotReel.transform.SetParent(transform, false);
            slotReel.transform.position += GameManager.Instance.GetSpriteGridSize() * i * Vector3.right;
            slotReel.OnReelFinalized += IncrementReelsFinalized;

            _slotReels.Add(slotReel);

            //Spawn Reel Symbols
            for (int j = 0; j < _gameConfig.Reels[i].SymbolConfigs.Length; j++)
            {
                SymbolConfig symbolConfig = _gameConfig.Reels[i].SymbolConfigs[j];

                GameObject symbolObject = new GameObject($"Reel{i+1} Symbol{j+1}");
                symbolObject.transform.SetParent(_slotReels[i].transform, false);
                symbolObject.transform.position += _gameManager.GetSpriteGridSize() * j * Vector3.down;

                SlotSymbol slotSymbol = symbolObject.AddComponent<SlotSymbol>();
                slotSymbol.Index = symbolConfig.Index;
                slotSymbol.Sprite = symbolConfig.Sprite;
                slotSymbol.Payout = symbolConfig.Payout;

                slotReel.SlotSymbols.Add(slotSymbol);

                //Duplicate Reel Symbols for seamless position reset
                if (j == _gameConfig.Reels[i].SymbolConfigs.Length - 1)
                {
                    for (int k = 0; k < _gameConfig.Reels[i].SymbolConfigs.Length; k++)
                    {
                        symbolConfig = _gameConfig.Reels[i].SymbolConfigs[k];

                        GameObject symbolObjectDupe = new GameObject($"Reel{i + 1} Symbol{k + 1}");
                        symbolObjectDupe.transform.SetParent(reelObject.transform);
                        symbolObjectDupe.transform.position = reelObject.transform.position + ((j + k + 1) * _gameManager.GetSpriteGridSize() * Vector3.down);

                        SlotSymbol slotSymbolDupe = symbolObjectDupe.AddComponent<SlotSymbol>();
                        slotSymbolDupe.Index = symbolConfig.Index;
                        slotSymbolDupe.Sprite = symbolConfig.Sprite;
                        slotSymbolDupe.Payout = symbolConfig.Payout;

                        slotReel.SlotSymbols.Add(slotSymbolDupe);
                    }
                }
            }
        }
    }

    void InitializeMaxBet()
    {
        _maxBet = 2000 * GameManager.Instance.GameConfig.PayoutLines.Length;
    }
    #endregion

    private void Start()
    {
        _gameConfig = GameManager.Instance.GameConfig;
        _gameManager = GameManager.Instance;

        InitializeSlotMachine();
        InitializeCamera();
        InitializeMaxBet();
    }

    #region PAYOUT_COMPUTATION
    void IncrementReelsFinalized()
    {
        ReelsFinalized++;

        if (ReelsFinalized >= _slotReels.Count)
        {
            ComputePayout();
        }
    }

    void ComputePayout()
    {
        int payout = 0;
        //Iterate through loaded payout line configs
        foreach(var config in GameManager.Instance.GameConfig.PayoutLines)
        {
            int matchingSymbols = 1;
            //Iterate through visible symbols per reel
            for (int i = 0; i < config.VisibleRow.Length; i++)
            {
                int indexToCheck = (i + 1 >= config.VisibleRow.Length) ? 0 : i + 1;

                //If no match and no streak, continue to next payout line config iteration
                if (_slotReels[i].VisibleSymbols[config.VisibleRow[i]].Index != _slotReels[indexToCheck].VisibleSymbols[config.VisibleRow[indexToCheck]].Index)
                {
                    if (matchingSymbols < 3)
                        break;
                }
                else
                {
                    matchingSymbols++;
                }

                //If there's a symbol streak, add payout value of symbol and number of matches to total payout
                if (i == config.VisibleRow.Length - 1)
                {
                    payout += _slotReels[i].VisibleSymbols[config.VisibleRow[i]].Payout[matchingSymbols];
                }
            }
        }
        int totalPayout = payout * _totalBet;

        _overlayUI._payoutLabel.text = totalPayout.ToString();
        _gameManager.UserData.Credits += totalPayout;
    }
    #endregion

    #region REELS_SPIN
    IEnumerator SpinReels()
    {
        //Check if player has enough credits
        if (_totalBet > _gameManager.UserData.Credits)
            yield break;

        _gameManager.UserData.Credits -= _totalBet;

        //Start Spinning Reels
        _isSpinning = true;
        ReelsFinalized = 0;

        foreach (var reel in _slotReels)
        {
            reel.StartSpin(GameManager.Instance.GetSpinSpeed());
        }

        //Wait until spin time configuration is reached
        float timeElapsed = 0;
        while (timeElapsed < GameManager.Instance.GameConfig.SpinTime)
        {
            timeElapsed += Time.fixedDeltaTime;
            yield return null;
        }

        StartCoroutine(StopReels());
    }

    IEnumerator StopReels()
    {
        //Stop reels one by one from the left
        foreach (var reel in _slotReels)
        {
            yield return new WaitForSeconds(Random.Range(0.25f, 0.75f));
            reel.StopSpin();
        }
        _isSpinning = false;
    }
    #endregion

    #region UI_BUTTONS
    public void IncrementBet()
    {
        _playerBet += 10;
        _totalBet = _playerBet * _gameConfig.PayoutLines.Length;

        if (_totalBet > _maxBet)
        {
            _totalBet = _maxBet;
            _playerBet = _totalBet / _gameConfig.PayoutLines.Length;
        }

        _overlayUI._betCreditsLabel.text = _totalBet.ToString();
    }

    public void DecrementBet()
    {
        _playerBet -= 10;
        _totalBet = _playerBet * _gameConfig.PayoutLines.Length;

        if (_totalBet <= 0)
        {
            _playerBet = 10;
            _totalBet = _playerBet * _gameConfig.PayoutLines.Length;
        }

        _overlayUI._betCreditsLabel.text = _totalBet.ToString();
    }

    public void StartSpin()
    {
        StartCoroutine(SpinReels());
    }
    #endregion
}
