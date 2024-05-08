// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakeCreditLoan : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TMP_InputField inputField;

    private int period;
    private int amount;
    private float monthlyRepayAmount;
    private float rate;
    private bool validate;

    public TextMeshProUGUI warning;
    public Button submitButton;

    public TextMeshProUGUI rateText;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI monthlyRepayAmountText;
    public TextMeshProUGUI periodText;

    void Start()
    {
        inputField.onValueChanged.AddListener(ValidateInput);  // Get the input value
        dropdown.onValueChanged.AddListener(OnDropdownChanged);  // Get the choice
        OnDropdownChanged(dropdown.value);
        validate = false;  // Initialize the validate to false
        submitButton.interactable = validate;
    }

    public void ValidateInput(string input)
    {
        if (int.TryParse(input, out int inputNumber))
        {
            if (inputNumber < 2000)
            {
                warning.text = "The minimum loan is 2000 yuan";
                validate = false;
            }
            else if (inputNumber > CreditRateManager.Instance.GetLoanLimit())
            {
                if (CreditRateManager.Instance.GetLoanLimit() < 2000)
                {
                    warning.text = "You can loan up to 0 yuan";
                }
                else
                {
                    warning.text = "You can loan up to " + CreditRateManager.Instance.GetLoanLimit() + " yuan";
                }
                validate = false;
            }
            else if (input.Length == 0)
            {
                warning.text = "";
                validate = false;
            }
            else
            {
                warning.text = "";
                validate = true;
                amount = inputNumber;
            }
        }

        submitButton.interactable = validate;

        if (validate)
        {
            ShowInfo();
        }
    }
    public void OnDropdownChanged(int index)
    {
        switch (index)
        {
            case 0:
                period = 12;
                break;
            case 1:
                period = 24;
                break;
            case 2:
                period = 36;
                break;
        }

        if (validate)
        {
            ShowInfo();
        }
    }

    public void ShowInfo()
    {
        rate = BankRateManager.Instance.GetCreditLoanRate(period);
        monthlyRepayAmount = (amount * rate * period / 12 + amount) / period;

        amountText.text = "Amount of the loan: " + amount;
        periodText.text = "Term of the loan: " + period;
        rateText.text = "Annual Interest Rate: " + (rate * 100).ToString() + "%";
        monthlyRepayAmountText.text = "Monthly Repayment: " + monthlyRepayAmount.ToString("F2");
    }

    public void SubmitLoan()
    {
        LoanManager.Instance.AddLoan(amount, rate, period, PlayerManager.Instance.GetRound());
        PlayerManager.Instance.AddPlayerCash(amount);
        PlayerManager.Instance.SetBankLoanValue();
    }
}
