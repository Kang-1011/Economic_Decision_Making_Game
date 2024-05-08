// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentDepositRateManager : MonoBehaviour
{
    BankRateManager rateManager;

    public TextMeshProUGUI dRate;

    void Awake()
    {
        rateManager = BankRateManager.Instance;
        UpdateDepositRate(rateManager.GetDepositRate_Current());
    }

    public void UpdateDepositRate(float rate)
    {
        dRate.text = "Rate : " + (rate * 100).ToString() + "%";
    }
}
