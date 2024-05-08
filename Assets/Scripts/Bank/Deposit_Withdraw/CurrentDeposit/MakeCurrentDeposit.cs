// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakeCurrentDeposit : MonoBehaviour
{
    public TMP_InputField inputField;
    private float amount;
    private bool validate;
    public float cash;
    public float rate;

    public TextMeshProUGUI warning;
    public Button submitButton;

    public TextMeshProUGUI amountText;
    public TextMeshProUGUI rateText;

    void Start()
    {
        cash = PlayerManager.Instance.GetPlayerCash();
        rate = BankRateManager.Instance.GetDepositRate_Current();
        inputField.onValueChanged.AddListener(ValidateInput);  // Get the input value
        validate = false;  // Initialize the validate to false
        submitButton.interactable = validate;
    }

    public void ValidateInput(string input)
    {
        if (float.TryParse(input, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float inputNumber))
        {
            if (inputNumber < 50f)
            {
                warning.text = "The minimum deposit is 50 yuan";
                validate = false;
            }
            else if (inputNumber > cash)
            {
                warning.text = "You don't have enough cash";
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

    public void ShowInfo()
    {
        // Show player's deposit information
        amountText.text = "Amount of the deposit: " + amount;
        rateText.text = "Annual Interest Rate: " + (rate * 100).ToString() + "%";
    }

    public void SubmitDeposit()
    {
        // Add the deposit to list and subtract player's money
        CurrentDepositManager.Instance.AddCurrentDeposit(amount, BankRateManager.Instance.GetDepositRate_Current(), PlayerManager.Instance.GetRound());
        PlayerManager.Instance.SubtractPlayerCash(amount);
        PlayerManager.Instance.SetBankDepositValue();
    }

    public bool GetValidate() { return validate; }
}
