// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoanData
{
    private int loanAmount;
    private float rate;
    private float repayAmount;
    private float monthlyRepayAmount;
    private int loanDate;
    private int period;
    private int finishDate;
    private float remainAmount;
    private bool[] isRepayed;
    private int repayedRound;
    private int defaultRound;
    private float defaultAmount;

    public LoanData(int loanAmount, float rate,int period, int loanDate)
    {
        this.loanAmount = loanAmount;
        this.rate = rate;
        this.period = period;
        this.loanDate = loanDate;
        finishDate = period + loanDate;
        repayAmount = loanAmount * rate * period / 12 + loanAmount;
        remainAmount = repayAmount;
        monthlyRepayAmount = repayAmount / period;
        repayedRound = 0;
        defaultRound = 0;
        defaultAmount = 0f;
        isRepayed = new bool[period];
        for (int i = 0; i < period; i++)
        {
            isRepayed[i] = false;
        }
    }

    public void UpdateRemainAmount()
    {
        remainAmount -= monthlyRepayAmount;
    }

    public void AddRepayedRound()
    {
        repayedRound += 1;
    }

    public void AddDefaultRound()
    {
        defaultRound += 1;
    }

    public void UpdateDefault()
    {
        defaultAmount += defaultRound * monthlyRepayAmount * rate / 6;
        remainAmount += defaultRound * monthlyRepayAmount * rate / 6;
        PlayerManager.Instance.SetBankLoanValue();
    }

    public void RepayDefault()
    {
        remainAmount = monthlyRepayAmount * (period - repayedRound);
        defaultAmount = 0f;
        defaultRound = 0;
    }

    public int GetDefaultRound()
    {
        return defaultRound;
    }

    public float GetDefaultAmount()
    {
        return defaultAmount;
    }
    
    public void FinishRepay()
    {
        if (repayedRound <= period)
        {
            isRepayed[repayedRound - 1] = true;
        }
        else
        {
            isRepayed[period - 1] = true;
        }
    }

    public int GetLoanAmount()
    {
        return loanAmount;
    }

    public float GetRate()
    {
        return rate;
    }

    public int GetPeriod()
    {
        return period;
    }

    public int GetLoanDate()
    {
        return loanDate;
    }

    public int GetFinishDate()
    {
        return finishDate;
    }

    public float GetRepayAmount()
    {
        return repayAmount;
    }

    public int GetRepayedRound()
    {
        return repayedRound;
    }

    public float GetMonthlyRepayAmount()
    {
        return monthlyRepayAmount;
    }

    public float GetRemainAmount()
    {
        return remainAmount;
    }

    public bool GetIsRepayed()
    {
        if (repayedRound <= period)
        {
            return isRepayed[repayedRound - 1];
        }
        else
        {
            return isRepayed[period - 1];
        }
    }

    public bool GetIsLastRoundRepayed()
    {
        if (repayedRound - 1 <= period)
        {
            return isRepayed[repayedRound - 2];
        }
        else
        {
            return isRepayed[period - 2];
        }
    }
}
