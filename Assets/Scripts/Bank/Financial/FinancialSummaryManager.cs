// All code was written by the team

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FinancialSummaryManager : MonoBehaviour
{
    public Text totalSpendingText;
    public Text totalIncomeText;
    public Text noPurchaseText;
    public Text Total;
    public GameObject Summary;
    public TextMeshProUGUI SummaryText;

    public float totalSpending;
    public float totalIncome;

    public float R1_Principal;
    public float R3_Principal;
    public float R5_Principal;

    public float R1_Income;
    public float R3_Income;
    public float R5_Income;

    public float R1_NetIncome;
    public float R3_NetIncome;
    public float R5_NetIncome;

    public List<FinancialData> financialDatas;
    public List<RedemptionData> redeemedDatas;

    private void Start()
    {
        financialDatas = FinancialManager.Instance.GetList();
        redeemedDatas = FinancialManager.Instance.GetRedeemedList();
        DisplayFinancialSummary();
    }

    public void DisplayFinancialSummary()
    {
        if (financialDatas.Count == 0 && redeemedDatas.Count == 0)
        {
            // Show "No Financial Product" message
            noPurchaseText.gameObject.SetActive(true);
            totalSpendingText.gameObject.SetActive(false);
            totalIncomeText.gameObject.SetActive(false);
            Total.gameObject.SetActive(false);
            Summary.gameObject.SetActive(false);
            return;
        }else{
            noPurchaseText.gameObject.SetActive(false);
            totalSpendingText.gameObject.SetActive(true);
            totalIncomeText.gameObject.SetActive(true);
            Total.gameObject.SetActive(true);
            Summary.gameObject.SetActive(true);
        }

        totalSpending = CalculateTotalSpending();
        totalIncome = CalculateTotalIncome();

        totalSpendingText.text = "￥ " + totalSpending.ToString("F2");
        totalIncomeText.text = "￥ " + totalIncome.ToString("F2");
        
        CalculateDiffTypeInfo();
        if(IsProfitable())
        {
            SummaryText.text = GetProfitableSummary();
        }else{
            SummaryText.text = GetLossSummary();
        }
    }

    public float CalculateTotalSpending()
    {
        float spending = 0f;

        foreach (FinancialData data in financialDatas)
        {
            spending += data.GetPrincipal();
        }

        foreach (RedemptionData data in redeemedDatas)
        {
            spending += data.GetPrincipal();
        }

        return spending;
    }

    public float CalculateTotalIncome()
    {
        float income = 0f;

        foreach (FinancialData data in financialDatas)
        {
            income += data.GetTotalAmount();
        }

        foreach (RedemptionData data in redeemedDatas)
        {
            income += data.GetTotalAmount();
        }

        return income;
    }

    public bool IsProfitable()
    {
        if(totalIncome > totalSpending)
        {
            return true;
        }else{
            return false;
        }
    }

    public void CalculateDiffTypeInfo()
    {
        foreach (FinancialData data in financialDatas)
        {
            if(data.GetFinancialType() == "R1")
            {
                R1_Principal += data.GetPrincipal();
                R1_Income += data.GetTotalAmount();
            }

            if(data.GetFinancialType() == "R3")
            {
                R3_Principal += data.GetPrincipal();
                R3_Income += data.GetTotalAmount();
            }

            if(data.GetFinancialType() == "R5")
            {
                R5_Principal += data.GetPrincipal();
                R5_Income += data.GetTotalAmount();
            }
        }

        foreach (RedemptionData data in redeemedDatas)
        {
            if(data.GetFinancialType() == "R1")
            {
                R1_Principal += data.GetPrincipal();
                R1_Income += data.GetTotalAmount();
            }

            if(data.GetFinancialType() == "R3")
            {
                R3_Principal += data.GetPrincipal();
                R3_Income += data.GetTotalAmount();
            }

            if(data.GetFinancialType() == "R5")
            {
                R5_Principal += data.GetPrincipal();
                R5_Income += data.GetTotalAmount();
            }
        }

        R1_NetIncome = R1_Income - R1_Principal;
        R3_NetIncome = R3_Income - R3_Principal;
        R5_NetIncome = R5_Income - R5_Principal;
    }

    public string GetProfitableSummary()
    {
        string text = "";

        if (R1_NetIncome > R3_NetIncome && R1_NetIncome > R5_NetIncome)
        {
            text = "Among all the financial products you've purchased, products with risk level R1 yield the highest net income, reaching "
                    + R1_NetIncome + " yuan.\n"
                    + "In economics, expected payoff refers to the anticipated average profit from an investment or business activity over a certain future period, taking into account various risk factors. In theory, the actual investment outcomes of low-risk products typically tend to be closer to their expected payoff. This is because low-risk products exhibit lower volatility and uncertainty, rendering them relatively stable.";
        }
        else if (R3_NetIncome > R1_NetIncome && R3_NetIncome > R5_NetIncome)
        {
            text = "Among all the financial products you've purchased, products with risk level R3 yield the highest net income, reaching "
                    + R3_NetIncome + " yuan.\n"
                    + "In economics, expected payoff refers to the anticipated average profit from an investment or business activity over a certain future period, taking into account various risk factors. Medium-risk products typically involve a certain degree of market volatility and uncertainty, albeit less pronounced than high-risk products, which may still impact investment outcomes to some extent. Consequently, deviations from expected payoff are also plausible.";
        }
        else if (R5_NetIncome > R1_NetIncome && R5_NetIncome > R3_NetIncome)
        {
            text = "Among all the financial products you've purchased, products with risk level R5 yield the highest net income, reaching "
                    + R5_NetIncome + " yuan.\n"
                    + "Products with Risk R5 entail the highest interest rates, yet they also pose the greatest potential for losses. In economics, expected payoff refers to the anticipated average profit from an investment or business activity over a certain future period, taking into account various risk factors. Generally, high-risk products tend to offer higher expected payoff. It's important to note that expected payoff is merely an anticipated value, and actual investment outcomes may differ.";
        }else{
            text = "";
        }

        return text;
    }

    public string GetLossSummary()
    {
        string text = "";

        if(R3_NetIncome < R5_NetIncome)
        {
            text = "In this module, the product with the highest loss is classified as R3 risk level, reaching "
            + Math.Abs(R3_NetIncome) + " yuan.\n"
            + "In economics, expected payoff refers to the anticipated average profit from an investment or business activity over a certain future period, taking into account various risk factors. Medium-risk products typically involve a certain degree of market volatility and uncertainty, albeit less pronounced than high-risk products, which may still impact investment outcomes to some extent. Consequently, deviations from expected payoff are also plausible.";
        }else if(R5_NetIncome < R3_NetIncome)
        {
            text = "In this module, the product with the highest loss is classified as R5 risk level, reaching "
            + Math.Abs(R5_NetIncome) + " yuan.\n"
            + "Products with Risk R5 entail the highest interest rates, yet they also pose the greatest potential for losses. In economics, expected payoff refers to the anticipated average profit from an investment or business activity over a certain future period, taking into account various risk factors. Due to their typically higher volatility and uncertainty, high-risk products may exhibit significant deviations from the expected returns in their actual performance.";
        }

        return text;
    }
}