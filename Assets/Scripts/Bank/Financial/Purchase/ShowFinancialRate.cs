// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowFinancialRate : MonoBehaviour
{
    public Text R1Rate;
    public Text R3Rate;
    public Text R5Rate;

    void Awake()
    {
        UpdateFinancialRate(FinancialRateManager.Instance.GetAverageRate_R1(), 
        FinancialRateManager.Instance.GetAverageRate_R3(), FinancialRateManager.Instance.GetAverageRate_R5());
    }

    public void UpdateFinancialRate(float rateR1, float rateR3, float rateR5)
    {
        R1Rate.text = (rateR1 * 100).ToString("F2") + "%";
        R3Rate.text = (rateR3 * 100).ToString("F2") + "%";
        R5Rate.text = (rateR5 * 100).ToString("F2") + "%";
    }
}
