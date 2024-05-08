// All code was written by the team

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using UnityEngine.SceneManagement;

public class PaymentManager : MonoBehaviour
{
    public HousePriceManager housePriceManager;
    public HouseRentManager houseRentManager;
    public int currentRound;

    public Text purchaseText;
    public Text noCashText;
    public Button purchaseBtn;
    public Text houseText;
    public Text areaText;
    public Text downPaymentRatioText;
    public Text downPaymentText;
    public Text loanText;
    public Text monthlyPaymentText;

    private char houseIdentifier;
    public float area;
    public float price;
    private float totalPrice;
    public float downPaymentRatio;
    public float downPayment;
    public float loan;
    public float annualInterestRate;
    private float monthlyInterestRate;
    public float monthlyPayment;
    public float loanPeriodsInMonths;

    public void Start()
    {
        currentRound = PlayerManager.Instance.GetRound();
        downPaymentRatio = 0.2f;
        annualInterestRate = 0.06f;
        loanPeriodsInMonths = 360;

        UpdateDownPaymentRatio();
        CalculateDownPaymentAndLoan();
        CalculateMonthlyPayment();
    }

    public void DecreaseDownPaymentRatio()
    {
        downPaymentRatio -= 0.1f;
        ClampDownPaymentRatio();
        UpdateDownPaymentRatio();
        CalculateDownPaymentAndLoan();
        CalculateMonthlyPayment();
    }

    public void IncreaseDownPaymentRatio()
    {
        downPaymentRatio += 0.1f;
        ClampDownPaymentRatio();
        UpdateDownPaymentRatio();
        CalculateDownPaymentAndLoan();
        CalculateMonthlyPayment();
    }

    public void ClampDownPaymentRatio()
    {
        // Ensure the ratio stays within the range [20%, 100%]
        downPaymentRatio = Mathf.Clamp(downPaymentRatio, 0.2f, 1.0f);
    }

    public void UpdateDownPaymentRatio()
    {
        downPaymentRatioText.text = (downPaymentRatio * 100).ToString("F0") + "%";
    }

    public void CalculateDownPaymentAndLoan()
    {
        if (float.TryParse(areaText.text, out float parsedArea))
        {
            area = parsedArea;
        }
        else
        {
            Debug.LogError("Failed to parse area from Text component!");
            return;
        }

        string house = houseText.text;
        if (!string.IsNullOrEmpty(house))
        {
            houseIdentifier = house[house.Length - 1];
        }
        else
        {
            Debug.LogError("House text is empty!");
        }

        switch (houseIdentifier)
        {
            case 'A':
                price = housePriceManager.prices[currentRound - 1][0];
                break;
            case 'B':
                price = housePriceManager.prices[currentRound - 1][1];
                break;
            case 'C':
                price = housePriceManager.prices[currentRound - 1][2];
                break;
            case 'D':
                price = housePriceManager.prices[currentRound - 1][3];
                break;
            default:
                Debug.LogError("Invalid House Identifier!");
                break;
        }

        totalPrice = price * area;
        downPayment = downPaymentRatio * totalPrice;
        loan = totalPrice - downPayment;

        // Update UI text components
        downPaymentText.text = downPayment.ToString("F0");
        loanText.text = loan.ToString("F0");
    }

    public void CalculateMonthlyPayment()
    {
        monthlyInterestRate = annualInterestRate / 12;

        // Use formula to calculate the monthly payment
        monthlyPayment = loan * monthlyInterestRate * Mathf.Pow(1 + monthlyInterestRate, loanPeriodsInMonths) / (Mathf.Pow(1 + monthlyInterestRate, loanPeriodsInMonths) - 1);

        // Update UI text components
        monthlyPaymentText.text = monthlyPayment.ToString("F0");
    }

    
    public void PurchaseHouse()
    {
        if (PlayerManager.Instance.playerCash >= downPayment)
        {
            PlayerManager.Instance.playerCash -= downPayment;
            PlayerManager.Instance.houseValue += totalPrice;
            PlayerManager.Instance.houseLoan += loan;
            PlayerManager.Instance.monthlyPayment += monthlyPayment;
            switch (houseText.text)
            {
                case "House A":
                    PlayerManager.Instance.monthlyRentIncome += houseRentManager.rents[currentRound - 1][0];
                    break;
                case "House B":
                    PlayerManager.Instance.monthlyRentIncome += houseRentManager.rents[currentRound - 1][1];
                    break;
                case "House C":
                    PlayerManager.Instance.monthlyRentIncome += houseRentManager.rents[currentRound - 1][2];
                    break;
                case "House D":
                    PlayerManager.Instance.monthlyRentIncome += houseRentManager.rents[currentRound - 1][3];
                    break;
                default:
                    break;
            }
            PlayerManager.Instance.preMonthlyRentIncome = PlayerManager.Instance.monthlyRentIncome;  
            House house = new House(currentRound, houseText.text, area, price, downPayment, monthlyPayment);
            purchaseText.enabled = true;
            int idx = Array.IndexOf(BtnManager.purchaseBtns, purchaseBtn);
            BtnManager.states[idx] = false;
        }
        else
        {
            noCashText.enabled = true;
        }
    }
    
}
