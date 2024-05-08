// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositManager : MonoBehaviour
{
    public static DepositManager Instance { get; private set; }

    public List<DepositData> depositDatas = new List<DepositData>();   // Store the data of deposits
    public List<DepositData> withdrawDatas = new List<DepositData>();   // Store the data of withdrawal deposits

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
    }

    // Add a new deposit
    public void AddDeposit(float principal, float rate, int period, int depositDate)
    {
        depositDatas.Add(new DepositData(principal, rate, period, depositDate));
    }

    // Remove the specific deposit
    public void RemoveDeposit(DepositData data)
    {
        depositDatas.Remove(data);
    }

    // Update the deposit interest and total amount at new round.
    public void UpdateDeposit()
    {
        for (int i = 0; i < depositDatas.Count; i++)
        {
            depositDatas[i].currentRound = PlayerManager.Instance.GetRound();
            depositDatas[i].currentDepositRate = BankRateManager.Instance.GetDepositRate_Current();
            depositDatas[i].UpdateInterest();
        }
        PlayerManager.Instance.SetBankDepositValue();
    }

    public float CalculateTotalAmount()
    {
        float totalAmount = 0;
        for (int i = 0; i < depositDatas.Count; i++)
        {
            totalAmount += depositDatas[i].GetTotalAmount();
        }
        return totalAmount;
    }

    // Add a withdrawal time deposit
    public void AddWithdrawalDeposit(DepositData data)
    {
        withdrawDatas.Add(data);
    }

    // Calculate the total principal of time deposit items
    public float CalculateTotalPrincipal()
    {
        float totalPrincipal = 0;
        for (int i = 0; i < withdrawDatas.Count; i++)
        {
            totalPrincipal += withdrawDatas[i].GetPrincipal();
        }
        for (int i = 0; i < depositDatas.Count; i++)
        {
            totalPrincipal += depositDatas[i].GetPrincipal();
        }
        return totalPrincipal;
    }

    // Calculate the total revenue of time deposit items
    public float CalculateTotalRevenue()
    {
        float totalRevenue = 0;
        for (int i = 0; i < withdrawDatas.Count; i++)
        {
            totalRevenue += withdrawDatas[i].GetTotalAmount();
        }
        for (int i = 0; i < depositDatas.Count; i++)
        {
            totalRevenue += depositDatas[i].GetTotalAmount();
        }
        return totalRevenue;
    }
}
