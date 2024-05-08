// All code was written by the team

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankRateManager : MonoBehaviour
{
    // Singleton Pattern
    public static BankRateManager Instance { get; private set; }

    private float depositRate_Current;
    private float depositRate_3Rounds;
    private float depositRate_6Rounds;
    private float depositRate_12Rounds;
    private float depositRate_24Rounds;
    private float depositRate_36Rounds;
    private float creditLoanRate_12Rounds;
    private float creditLoanRate_24Rounds;
    private float creditLoanRate_36Rounds;

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

        InitializeBankRates();
    }

    public void InitializeBankRates()
    {
        depositRate_Current = 0.0035f;
        depositRate_3Rounds = 0.0135f;
        depositRate_6Rounds = 0.0155f;
        depositRate_12Rounds = 0.0175f;
        depositRate_24Rounds = 0.0225f;
        depositRate_36Rounds = 0.0275f;
        creditLoanRate_12Rounds = 0.061f;
        creditLoanRate_24Rounds = 0.0645f;
        creditLoanRate_36Rounds = 0.0685f;
    }

    public float GetDepositRate_Current()
    {
        return depositRate_Current;
    }

    public void SetDepositRate_Current(float rate)
    {
        depositRate_Current = rate;
    }

    public float GetDepositRate_3Rounds()
    {
        return depositRate_3Rounds;
    }

    public void SetDepositRate_3Rounds(float rate)
    {
        depositRate_3Rounds = rate;
    }

    public float GetDepositRate_6Rounds()
    {
        return depositRate_6Rounds;
    }

    public void SetDepositRate_6Rounds(float rate)
    {
        depositRate_6Rounds = rate;
    }

    public float GetDepositRate_12Rounds()
    {
        return depositRate_12Rounds;
    }

    public void SetDepositRate_12Rounds(float rate)
    {
        depositRate_12Rounds = rate;
    }

    public float GetDepositRate_24Rounds()
    {
        return depositRate_24Rounds;
    }

    public void SetDepositRate_24Rounds(float rate)
    {
        depositRate_24Rounds = rate;
    }

    public float GetDepositRate_36Rounds()
    {
        return depositRate_36Rounds;
    }

    public void SetDepositRate_36Rounds(float rate)
    {
        depositRate_36Rounds = rate;
    }

    public float GetCreditLoanRate_12Rounds()
    {
        return creditLoanRate_12Rounds;
    }

    public void SetCreditLoanRate_12Rounds(float rate)
    {
        creditLoanRate_12Rounds = rate;
    }

    public float GetCreditLoanRate_24Rounds()
    {
        return creditLoanRate_24Rounds;
    }

    public void SetCreditLoanRate_24Rounds(float rate)
    {
        creditLoanRate_24Rounds = rate;
    }

    public float GetCreditLoanRate_36Rounds()
    {
        return creditLoanRate_36Rounds;
    }

    public void SetCreditLoanRate_36Rounds(float rate)
    {
        creditLoanRate_36Rounds = rate;
    }

    public float GetCreditLoanRate(int round)
    {
        switch (round)
        {
            case 12:
                return creditLoanRate_12Rounds;
            case 24:
                return creditLoanRate_24Rounds;
            case 36:
                return creditLoanRate_36Rounds;
            default:
                return 0;
        }
    }
}
