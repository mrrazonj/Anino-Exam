using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region SINGLETON_FORMAT

    public static GameManager Instance;

    private bool InitializeSingleton()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return false;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        return true;
    }

    #endregion
    #region DATA_INITIALIZATION

    public GameConfig GameConfig;

    public UnityAction OnCreditsChanged;
    public UserData UserData;

    #endregion

    private void Awake()
    {
        if (InitializeSingleton() == false)
            return;

        Application.runInBackground = true;
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    public float GetSpriteGridSize()
    {
        return GameConfig.Reels[0].SymbolConfigs[0].Sprite.bounds.size.x;
    }

    public float GetSpinSpeed()
    {
        return Random.Range(GameConfig.SpinSpeed * 0.33f, GameConfig.SpinSpeed * 3);
    }
}

[System.Serializable]
public class UserData
{
    public int Credits
    {
        get
        {
            return _credits;
        }
        set
        {
            _credits = value;
            GameManager.Instance.OnCreditsChanged?.Invoke();
        }
    }
    public int _credits;
}
