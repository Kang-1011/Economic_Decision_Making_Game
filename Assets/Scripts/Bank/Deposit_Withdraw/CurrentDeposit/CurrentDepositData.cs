// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrentDepositData
{
    // Data type of current deposit
    private float principal;
    private float interest;
    private float totalAmount;
    private float rate;
    private int depositDate;
    public CurrentDepositData(float principal, float rate, int depositDate)
    {
        this.principal = principal;
        this.rate = rate;
        this.depositDate = depositDate;
        interest = 0;
        UpdateTotalAmount();
    }
    
    public void UpdateTotalAmount()
    {
        totalAmount = interest + principal;
    }

    
    // Update Interest at new round
    public void UpdateInterest()
    {
        interest += principal * rate / 12;
        UpdateTotalAmount();
    }

    // Getters and Setters
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

    public float GetRate()
    {
        return rate;
    }

    public int GetDepositDate()
    {
        return depositDate;
    }
}
