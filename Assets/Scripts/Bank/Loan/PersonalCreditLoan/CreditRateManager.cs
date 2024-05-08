// All code was written by the team

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditRateManager : MonoBehaviour
{
    public static CreditRateManager Instance { get; private set; }

    private string creditRating;
    private int defaultTimes;
    private int defaultScore;
    private int negativeScore;
    private int assetScore;
    private int creditScore;
    private int loanLimit;
    private int maxLoanAmount;
    private int ontimeRepayTimes;

    void Awake()
    {
        // Initialize the singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        InitializeCreditRate();
    }

    private void InitializeCreditRate()
    {
        defaultTimes = 0;
        defaultScore = 40;
        negativeScore = 0;
        assetScore = 0;
        loanLimit = 0;
        maxLoanAmount = 0;
    }

    public void UpdateCreditRating()
    {
        int oldMaxLoanAmount = maxLoanAmount;
        int times = defaultTimes;
        for (int i = 0; i < times; i++)
        {
            if (defaultScore - 20 >= 0)
            {
                defaultScore -= 20;
                defaultTimes--;
            }
            else
            {
                negativeScore += (20 - defaultScore);
                defaultScore = 0;
                defaultTimes--;
            }
        }
        HandleOntimeRepay();

        float netAsset = PlayerManager.Instance.GetNetAsset();

        if (netAsset < 10000f)
        {
            assetScore = 0;
        }
        else if (netAsset >= 10000f && netAsset < 1000000f)
        {
            assetScore = (int)Math.Round((netAsset - 10000f) * 20 / 990000f);
        }
        else if (netAsset >= 1000000f && netAsset < 3000000f)
        {
            assetScore = 20 + (int)Math.Round((netAsset - 1000000f) * 20 / 2000000f);
        }
        else if (netAsset >= 3000000f && netAsset < 5000000f)
        {
            assetScore = 40 + (int)Math.Round((netAsset - 3000000f) * 10 / 2000000f);
        }
        else if (netAsset >= 5000000f && netAsset < 10000000f)
        {
            assetScore = 50 + (int)Math.Round((netAsset - 5000000f) * 10 / 5000000f);
        }
        else
        {
            assetScore = 60;
        }

        creditScore = assetScore + defaultScore;

        if (creditScore <= 30)
        {
            creditRating = "E";
        }
        else if (creditScore > 30 && creditScore <= 40)
        {
            creditRating = "D";
        }
        else if (creditScore > 40 && creditScore <= 50)
        {
            creditRating = "C";
        }
        else if (creditScore > 50 && creditScore <= 55)
        {
            creditRating = "CC";
        }
        else if (creditScore > 55 && creditScore <= 60)
        {
            creditRating = "CCC";
        }
        else if (creditScore > 60 && creditScore <= 70)
        {
            creditRating = "B";
        }
        else if (creditScore > 70 && creditScore <= 75)
        {
            creditRating = "BB";
        }
        else if (creditScore > 75 && creditScore <= 80)
        {
            creditRating = "BBB";
        }
        else if (creditScore > 80 && creditScore <= 90)
        {
            creditRating = "A";
        }
        else if (creditScore > 90 && creditScore < 100)
        {
            creditRating = "AA";
        }
        else
        {
            creditRating = "AAA";
        }

        maxLoanAmount = GetMaxLoanAmount(creditRating);
        loanLimit = maxLoanAmount - LoanManager.Instance.CalculateTotalLoanAmount();
    }

    public int GetMaxLoanAmount(string creditRating)
    {
        switch (creditRating)
        {
            case "C":
                return 10000;
            case "CC":
                return 50000;
            case "CCC":
                return 70000;
            case "B":
                return 100000;
            case "BB":
                return 250000;
            case "BBB":
                return 350000;
            case "A":
                return 500000;
            case "AA":
                return 700000;
            case "AAA":
                return 1000000;
            default:
                return 0;
        }
    }

    private void HandleOntimeRepay()
    {
        int times = ontimeRepayTimes;
        for (int i = 0; i < times; i++)
        {
            if(negativeScore > 0)
            {
                negativeScore--;
                ontimeRepayTimes--;
            }
            else if(defaultScore < 100)
            {
                defaultScore++;
                ontimeRepayTimes--;
            }
            else
            {
                ontimeRepayTimes--;
            }
        }
    }

    public void AddDefaultTime()
    {
        defaultTimes += 1;
    }

    public void OntimeRepay()
    {
        ontimeRepayTimes++;
    }

    public string GetCreditRating()
    {
        return creditRating;
    }

    public int GetCreditScore()
    {
        return creditScore;
    }

    public int GetLoanLimit()
    {
        return loanLimit;
    }

    public void AddLoanLimit(int amount)
    {
        loanLimit += amount;
    }

    public void SubtractLoanLimit(int amount)
    {
        loanLimit -= amount;
    }
}
