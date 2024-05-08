// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinancialManager : MonoBehaviour
{
    public static FinancialManager Instance { get; private set; }

    public List<FinancialData> financialDatas = new List<FinancialData>();   // Store the data of financial products
    public List<RedemptionData> redeemedDatas = new List<RedemptionData>();  // Store the data of redeemed products

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

    // Add a new financial product deal
    public void AddFinancial(string name, float principal, string type, int purchaseDate)
    {
        financialDatas.Add(new FinancialData(name, principal, type, purchaseDate));
    }

    public FinancialData FindRedeemedProduct(int index)
    {
        return financialDatas[index];
    }

    public void AddRedeemedProduct(FinancialData redeemedProduct)
    {
        float redeemedPrincipal = redeemedProduct.GetPrincipal();
        string redeemedType = redeemedProduct.GetFinancialType();
        float redeemedAmount = redeemedProduct.GetTotalAmount();
        redeemedDatas.Add(new RedemptionData(redeemedPrincipal, redeemedType, redeemedAmount));
    }

    public void AddRedeemedProduct(float principal, string type, float totalAmount)
    {
        redeemedDatas.Add(new RedemptionData(principal, type, totalAmount));
    }

    public void RemoveFinancial(int index)
    {
        financialDatas.RemoveAt(index);
    }

    public void RedeemPartially(int index, float amount)
    {
        FinancialData temp = new FinancialData(financialDatas[index]);
        financialDatas[index].RedeemPartially(amount);
        float principal = temp.GetPrincipal() - financialDatas[index].GetPrincipal();
        string type = temp.GetFinancialType();
        AddRedeemedProduct(principal, type, amount);
    }

    // Update the interests
    public void UpdateFinancialInterests()
    {
        for (int i = 0; i < financialDatas.Count; i++)
        {
            financialDatas[i].rate = FinancialRateManager.Instance.GetCurrentRate(financialDatas[i].GetFinancialType());
            financialDatas[i].UpdateInterest();
        }
        PlayerManager.Instance.SetBankFinanceValue();
    }

    public float CalculateTotalAmount()
    {
        float totalAmount = 0;
        for (int i = 0; i < financialDatas.Count; i++)
        {
            totalAmount += financialDatas[i].GetTotalAmount();
        }
        return totalAmount;
    }

    public List<FinancialData> GetList()
    {
        return financialDatas;
    }

    public List<RedemptionData> GetRedeemedList()
    {
        return redeemedDatas;
    }
}
