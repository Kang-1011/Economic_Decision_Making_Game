// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseFinancial : MonoBehaviour
{
    public TMP_InputField inputField;
    public Toggle toggle;
    public TextMeshProUGUI level;
    public TextMeshProUGUI warning;

    public TextMeshProUGUI productName;
    private float amount;
    public string type;

    public Button submitButton;

    private bool validateAmount;
    private bool validatePrinciple;
    private bool validate;

    public float miniAmount;
    public float cash;
    public float netAsset;

    public TextMeshProUGUI amountText;
    public TextMeshProUGUI typeText;

    void Start()
    {
        cash = PlayerManager.Instance.GetPlayerCash();
        netAsset = PlayerManager.Instance.GetNetAsset();
        type = GetFinancialType();
        inputField.onValueChanged.AddListener(ValidateInput);
        toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(toggle); });
        validateAmount = false;
        validatePrinciple = false;
        validate = false;
        submitButton.interactable = validate;
    }

    private string GetFinancialType()
    {
        if (level.text == "Level: R1")
        {
            miniAmount = 100f;
            return "R1";
        }
        else if (level.text == "Level: R3")
        {
            miniAmount = 1000f;
            return "R3";
        }
        else
        {
            miniAmount = 5000f;
            return "R5";
        }
    }
    
    public void ValidateInput(string input)
    {
        if (type == "R5" && netAsset < 1001000)
        {
            warning.text = "You can not purchase this product until you get a net asset of 1001000!";
            validate = false;
        }
        else
        {
            if (float.TryParse(input, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float inputNumber))
            {
                if (inputNumber < miniAmount)
                {
                    warning.text = "The minimum amount is " + miniAmount + " yuan";
                    validateAmount = false;
                    CheckValidate();
                }
                else if (inputNumber > cash)
                {
                    warning.text = "You don't have enough cash";
                    validateAmount = false;
                    CheckValidate();
                }
                else if (input.Length == 0)
                {
                    warning.text = "";
                    validateAmount = false;
                    CheckValidate();
                }
                else
                {
                    warning.text = "";
                    validateAmount = true;
                    CheckValidate();
                    amount = inputNumber;
                }
            }
        }

        submitButton.interactable = validate;
        if (validate)
        {
            ShowInfo();
        }
    }

    private void ShowInfo()
    {
        amountText.text = "Amount: " + amount;
        typeText.text = "Level: " + type;
    }

    public void ToggleValueChanged(Toggle change)
    {
        validatePrinciple = change.isOn;
        CheckValidate();
        submitButton.interactable = validate;
        if (validate)
        {
            ShowInfo();
        }
    }

    private void CheckValidate()
    {
        validate = validateAmount && validatePrinciple;
    }

    public void SubmitFinancial()
    {
        FinancialManager.Instance.AddFinancial(productName.text, amount, type, PlayerManager.Instance.GetRound());
        PlayerManager.Instance.SubtractPlayerCash(amount);
        PlayerManager.Instance.SetBankFinanceValue();
    }

    public bool GetValidateAmount() { return validateAmount; }
    public bool GetValidate() { return validate; }
}
