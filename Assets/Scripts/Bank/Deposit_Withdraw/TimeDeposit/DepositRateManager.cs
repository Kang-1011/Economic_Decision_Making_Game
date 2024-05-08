// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DepositRateManager : MonoBehaviour
{
    BankRateManager rateManager;

    public TextMeshProUGUI dRate_3Rounds;
    public TextMeshProUGUI dRate_6Rounds;
    public TextMeshProUGUI dRate_12Rounds;
    public TextMeshProUGUI dRate_24Rounds;
    public TextMeshProUGUI dRate_36Rounds;

    void Awake()
    {
        rateManager = BankRateManager.Instance;
        UpdateDepositRate(rateManager.GetDepositRate_3Rounds(), rateManager.GetDepositRate_6Rounds(),
                            rateManager.GetDepositRate_12Rounds(), rateManager.GetDepositRate_24Rounds(), 
                            rateManager.GetDepositRate_36Rounds());
    }

    public void UpdateDepositRate(float rate_3Rounds, float rate_6Rounds, float rate_12Rounds, float rate_24Rounds, float rate_36Rounds)
    {
        dRate_3Rounds.text = "3 Rounds: " + (rate_3Rounds * 100).ToString() + "%";
        dRate_6Rounds.text = "6 Rounds: " + (rate_6Rounds * 100).ToString() + "%";
        dRate_12Rounds.text = "12 Rounds: " + (rate_12Rounds * 100).ToString() + "%";
        dRate_24Rounds.text = "24 Rounds: " + (rate_24Rounds * 100).ToString() + "%";
        dRate_36Rounds.text = "36 Rounds: " + (rate_36Rounds * 100).ToString() + "%";
    }
}
