// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakeDeposit : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TMP_InputField inputField;
    private int period;
    private float amount;
    private bool validate;
    public float cash;
    public float[] timeRate;

    public TextMeshProUGUI warning;
    public Button submitButton;

    public TextMeshProUGUI periodText;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI rateText;

    void Start()
    {
        cash = PlayerManager.Instance.GetPlayerCash();
        timeRate = new float[5] {BankRateManager.Instance.GetDepositRate_3Rounds(), BankRateManager.Instance.GetDepositRate_6Rounds(),
                    BankRateManager.Instance.GetDepositRate_12Rounds(), BankRateManager.Instance.GetDepositRate_24Rounds(),
                    BankRateManager.Instance.GetDepositRate_36Rounds()};

        inputField.onValueChanged.AddListener(ValidateInput);  // Get the input value
        dropdown.onValueChanged.AddListener(OnDropdownChanged);  // Get the choice
        OnDropdownChanged(dropdown.value);
        validate = false;  // Initialize the validate to false
        submitButton.interactable = validate;
    }

    public void OnDropdownChanged(int index)
    {
        switch (index)
        {
            case 0:
                period = 3;
                break;
            case 1:
                period = 6;
                break;
            case 2:
                period = 12;
                break;
            case 3:
                period = 24;
                break;
            case 4:
                period = 36;
                break;
        }

        if (validate)
        {
            ShowInfo();
        }
    }

    public void ValidateInput(string input)
    {
        if(float.TryParse(input, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float inputNumber))
        {
            if(inputNumber < 50f)
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
        periodText.text = "Period of the deposit: " + period;
        amountText.text = "Amount of the deposit: " + amount;
        rateText.text = "Annual Interest Rate: " + (CalculateRate(period) * 100).ToString() + "%";
    }

    private float CalculateRate(int period)
    {
        // Get the deposit rate
        float rate;
        switch(period)
        {
            case 3:
                rate = timeRate[0];
                break;
            case 6:
                rate = timeRate[1];
                break;
            case 12:
                rate = timeRate[2];
                break;
            case 24:
                rate = timeRate[3];
                break;
            case 36:
                rate = timeRate[4];
                break;
            default:
                rate = 0.0f;
                break;
        }
        return rate;
    }

    public void SubmitDeposit()
    {
        // Add the deposit to list and subtract player's money
        DepositManager.Instance.AddDeposit(amount, CalculateRate(period), period, PlayerManager.Instance.GetRound());
        PlayerManager.Instance.SubtractPlayerCash(amount);
        PlayerManager.Instance.SetBankDepositValue();
    }

    // Getter
    public bool GetValidate() { return validate; }
    public int GetPeriod() { return period; }
}
