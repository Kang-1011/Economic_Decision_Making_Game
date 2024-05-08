// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Singleton Pattern
    public static PlayerManager Instance { get; private set; }

    private int round; // Store the current round
    public float playerCash; // Store the cash amount of the player
    public float houseValue;
    public float houseLoan;
    public float preMonthlyRentIncome;
    public float monthlyRentIncome;
    public float monthlyPayment;
    private float stockValue; // Store the value of stock
    private float futuresValue; // Store the value of futures
    private float bankDepositValue; // Store the value of bank deposit
    private float bankFinanceValue; // Store the value of bank finance
    private float bankLoanValue; // Store the value of bank loan

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

        InitializePlayerAssets();
    }

    public void InitializePlayerAssets()
    {
        round = 1;
        playerCash = 1000000.0f;
        houseValue = 0.0f;
        stockValue = 0.0f;
        futuresValue = 0.0f;
        bankDepositValue = 0.0f;
        bankFinanceValue = 0.0f;
        bankLoanValue = 0.0f;
        monthlyPayment = 0.0f;
    }

    public int GetRound()
    {
        return round;
    }

    public void AddRound()
    {
        round++;
    }

    public float GetPlayerCash()
    {
        return playerCash;
    }

    public void AddPlayerCash(float addAmount)
    {
        playerCash += addAmount;
    }

    public void SubtractPlayerCash(float subtractAmount)
    {
        playerCash -= subtractAmount;
    }

    public float GetStockValue()
    {
        return stockValue;
    }

    public void SetStockValue(float amount)
    {
        stockValue = amount;
    }

    public float GetFuturesValue()
    {
        return futuresValue;
    }

    public void SetFuturesValue(float amount)
    {
        futuresValue = amount;
    }

    public float GetBankDepositValue()
    {
        return bankDepositValue;
    }

    public void SetBankDepositValue()
    {
        bankDepositValue = DepositManager.Instance.CalculateTotalAmount() + CurrentDepositManager.Instance.CalculateTotalAmount();
    }

    public float GetBankFinanceValue()
    {
        return bankFinanceValue;
    }

    public void SetBankFinanceValue()
    {
        bankFinanceValue = FinancialManager.Instance.CalculateTotalAmount();
    }

    public float GetBankLoanValue()
    {
        return bankLoanValue;
    }

    public void SetBankLoanValue()
    {
        bankLoanValue = LoanManager.Instance.CalculateTotalLoan();
    }

    public float GetNetAsset()
    {
        return (playerCash + bankDepositValue + bankFinanceValue - bankLoanValue + futuresValue + houseValue + stockValue);
    }

    public void UpdateHouseValue()
    {
        HousePriceManager housePriceManager;
        GameObject housePriceManagerOb = new GameObject();
        housePriceManager = housePriceManagerOb.AddComponent<HousePriceManager>();
        housePriceManager.Start();
        houseValue = 0;
        foreach (House house in House.purchasedHouses)
        {
            switch (house.title)
            {
                case "House A":
                    houseValue += housePriceManager.prices[round - 1][0] * house.area;
                    break;
                case "House B":
                    houseValue += housePriceManager.prices[round - 1][1] * house.area;
                    break;
                case "House C":
                    houseValue += housePriceManager.prices[round - 1][2] * house.area;
                    break;
                case "House D":
                    houseValue += housePriceManager.prices[round - 1][3] * house.area;
                    break;
                default:
                    break;
            }
        }
        GameObject.DestroyImmediate(housePriceManager.gameObject);
    }

    public void UpdateHouseLoan()
    {
        houseLoan = 0;
        foreach (House house in House.purchasedHousesHistory)
        {
            houseLoan += house.purchasePrice * house.area - house.downPayment - (round - house.round) * house.monthlyPayment;
        }
    }

    public void UpdateMonthlyRentIncome()
    {
        HouseRentManager houseRentManager;
        GameObject houseRentManagerOb = new GameObject();
        houseRentManager = houseRentManagerOb.AddComponent<HouseRentManager>();
        houseRentManager.Start();
        monthlyRentIncome = 0;
        foreach (House house in House.purchasedHouses)
        {
            switch (house.title)
            {
                case "House A":
                    monthlyRentIncome += houseRentManager.rents[PlayerManager.Instance.GetRound() - 1][0];
                    break;
                case "House B":
                    monthlyRentIncome += houseRentManager.rents[PlayerManager.Instance.GetRound() - 1][1];
                    break;
                case "House C":
                    monthlyRentIncome += houseRentManager.rents[PlayerManager.Instance.GetRound() - 1][2];
                    break;
                case "House D":
                    monthlyRentIncome += houseRentManager.rents[PlayerManager.Instance.GetRound() - 1][3];
                    break;
                default:
                    break;
            }
        }
        preMonthlyRentIncome = monthlyRentIncome;
        GameObject.DestroyImmediate(houseRentManager.gameObject);
    }
}
