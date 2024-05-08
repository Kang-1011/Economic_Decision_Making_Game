// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DepositData
{
    // Data type of time deposit
    private float principal;
    private float interest;
    private float totalAmount;
    private float rate;
    private int period;
    private int depositDate;
    private int finishDate;
    private bool due;
    private bool expire;
    public int currentRound;
    public float currentDepositRate;

    public DepositData(float principal, float rate, int period, int depositDate)
    {
        this.principal = principal;
        this.rate = rate;
        this.period = period;
        this.depositDate = depositDate;
        interest = 0;
        due = false;
        expire = false;
        UpdateTotalAmount();
        finishDate = period + depositDate;
    }

    public void UpdateTotalAmount()
    {
        totalAmount = interest + principal;
    }

    // Change some amounts at the time of early withdrawal
    public void EarlyWithdraw()
    {   
        rate = currentDepositRate; // Current deposit interest rate
        interest = principal * (rate / 12) * (currentRound - depositDate); // Total Interest
        totalAmount = interest + principal;
    }

    // Update Interest at new round without early withdrawal
    public void UpdateInterest()
    {
        CheckExpireDue();
        // If the time deposit has expired, the type of this deposit becomes current deposit
        if (expire == true && due == false)
        {
            rate = currentDepositRate;
        }
        interest += principal * rate / 12;
        UpdateTotalAmount();
    }

    // Check whether the deposit has expired and is due
    public void CheckExpireDue()
    {
        if (currentRound > finishDate)
        {
            expire = true;
            due = false;
        }
        else if (currentRound == finishDate)
        {
            due = true;
        }
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

    public int GetPeriod()
    {
        return period;
    }

    public int GetDepositDate()
    {
        return depositDate;
    }

    public int GetFinishDate()
    {
        return finishDate;
    }

    public bool GetDue()
    {
        return due;
    }

    public bool GetExpire()
    {
        return expire;
    }
}
