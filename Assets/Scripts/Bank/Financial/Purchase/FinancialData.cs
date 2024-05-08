// All code was written by the team

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinancialData
{
    private float principal;
    private float interest;
    private float totalAmount;
    private string type;
    private int purchaseDate;
    private string name;
    public float rate;

    public FinancialData(string name, float principal, string type, int purchaseDate)
    {
        this.name = name;
        this.principal = principal;
        this.type = type;
        this.purchaseDate = purchaseDate;
        interest = 0;
        UpdateTotalAmount();
    }

    // Copy financial data.
    public FinancialData(FinancialData original)
    {
        this.name = original.name;
        this.principal = original.principal;
        this.type = original.type;
        this.purchaseDate = original.purchaseDate;
        this.interest = original.interest;
        this.totalAmount = original.totalAmount;
    }

    public void SetTotalAmount(float amount)
    {
        totalAmount = amount;
    }

    public void UpdateTotalAmount()
    {
        principal = (float)Math.Round(principal, 2);
        interest = (float)Math.Round(interest, 2);
        totalAmount = principal + interest;
    }

    public string GetName()
    {
        return name;
    }

    public float GetPrincipal()
    {
        return principal;
    }

    public float GetInterest()
    {
        return interest;
    }

    public float GetTotalAmount()
    {
        return totalAmount;
    }

    public string GetFinancialType()
    {
        return type;
    }

    public int GetPurchaseDate()
    {
        return purchaseDate;
    }

    public void RedeemPartially(float amount)
    {
        float percentage = (totalAmount - amount) / totalAmount;
        principal *= percentage;
        interest *= percentage;
        UpdateTotalAmount();
    }

    public void UpdateInterest()
    {
        // interest += principal * FinancialRateManager.Instance.GetCurrentRate(type);
        interest += principal * rate;
        UpdateTotalAmount();
    }
}
