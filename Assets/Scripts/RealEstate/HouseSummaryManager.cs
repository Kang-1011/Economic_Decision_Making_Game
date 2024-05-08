// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseSummaryManager : MonoBehaviour
{
    public Text netProfitText;
    public float netProfit;

    public void Start()
    {
        netProfit = 0;
        CalculateNetProfit();
    }

    public void CalculateNetProfit()
    {
        foreach (House house in House.purchasedHousesHistory)
        {
            netProfit += house.profitAndLoss;
        }
        netProfitText.text = netProfit.ToString("F0");
    }

}
