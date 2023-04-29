using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverlayUI : MonoBehaviour
{
    public Button SpinButton;
    public Button DecrementBetButton;
    public Button IncrementBetButton;
    public TextMeshProUGUI _betCreditsLabel;
    public TextMeshProUGUI _payoutLabel;

    [SerializeField]
    private TextMeshProUGUI _creditsLabel;

    private void Start()
    {
        GameManager.Instance.OnCreditsChanged += UpdateCredits;
        UpdateCredits();
    }

    void UpdateCredits()
    {
        _creditsLabel.text = GameManager.Instance.UserData.Credits.ToString();
    }
}
