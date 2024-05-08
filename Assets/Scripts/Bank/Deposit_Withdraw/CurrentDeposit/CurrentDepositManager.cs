// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentDepositManager : MonoBehaviour
{
    public static CurrentDepositManager Instance { get; private set; }

    public List<CurrentDepositData> depositDatas = new List<CurrentDepositData>();   // Store the data of current deposits
    public List<CurrentDepositData> withdrawDatas = new List<CurrentDepositData>();   // Store the data of withdrawal deposits

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

    // Add a new current deposit
    public void AddCurrentDeposit(float principal, float rate, int depositDate)
    {
        depositDatas.Add(new CurrentDepositData(principal, rate, depositDate));
    }

    // Remove the specific deposit
    public void RemoveDeposit(CurrentDepositData data)
    {
        depositDatas.Remove(data);
    }

    // Update the deposit interest and total amount at new round.
    public void UpdateDeposit()
    {
        for (int i = 0; i < depositDatas.Count; i++)
        {
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

    // Add a withdrawal current deposit
    public void AddWithdrawalCurrentDeposit(CurrentDepositData data)
    {
        withdrawDatas.Add(data);
    }

    // Calculate the total principal of current deposit items
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

    // Calculate the total revenue of current deposit items
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
