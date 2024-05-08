// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// using UnityEditorInternal.Profiling.Memory.Experimental;

public class RedemptionManager : MonoBehaviour
{
    public GameObject redemptionItemPrefab;
    public Transform content;
    public GameObject dialog;
    public GameObject dialogFail;

    public Button confirmButton;

    public TMP_InputField inputField;

    private string level;
    private float maxRedemptionAmount;
    private float redemptionAmount;
    private int purchaseDate;
    private int index;
    private int redeemAvailableDate = 0;

    private bool validate = true;
    private bool redeemType = true;

    public TextMeshProUGUI warningText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI failText;
    public Text noFinancialPurchased;

    void Start()
    {
        ShowAllProducts();
    }

    public void ShowAllProducts()
    {
        List<FinancialData> financialDatas = FinancialManager.Instance.GetList();

        if (financialDatas.Count == 0)
        {
            // Show "No Financial Product" message
            noFinancialPurchased.gameObject.SetActive(true);
            return;
        }else{
            noFinancialPurchased.gameObject.SetActive(false);
        }
        
        int i = 0;
        foreach (FinancialData data in financialDatas)
        {
            GameObject newItem = Instantiate(redemptionItemPrefab, content);
            TextMeshProUGUI name = FindTextComponentByName(newItem, "Name");
            TextMeshProUGUI principal = FindTextComponentByName(newItem, "Principal");
            TextMeshProUGUI level = FindTextComponentByName(newItem, "Level");
            TextMeshProUGUI totalAmount = FindTextComponentByName(newItem, "TotalAmount");
            TextMeshProUGUI purchaseDate = FindTextComponentByName(newItem, "PurchaseDate");
            TextMeshProUGUI priceChange = FindTextComponentByName(newItem, "PriceChange");

            Button button = FindButtonComponentByName(newItem, "Button");
            int index = i;
            if (button != null)
            {
                button.onClick.AddListener(() => ButtonClicked(index));
            }
            float change = data.GetInterest() / data.GetPrincipal() * 100;

            name.text = "Name: " + data.GetName();
            principal.text = "Principal: " + data.GetPrincipal().ToString("F2");
            level.text = "Level: " + data.GetFinancialType();
            totalAmount.text = "Total Amount: " + data.GetTotalAmount().ToString("F2");
            purchaseDate.text = "Purchase Date: Round " + data.GetPurchaseDate().ToString();
            priceChange.text = "Price Change: " + change.ToString("F2") + "%";
            i++;
        }
    }

    TextMeshProUGUI FindTextComponentByName(GameObject parent, string name)
    {
        TextMeshProUGUI[] allTextComponents = parent.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textComp in allTextComponents)
        {
            if (textComp.gameObject.name == name)
            {
                return textComp;
            }
        }
        return null;
    }

    Button FindButtonComponentByName(GameObject parent, string name)
    {
        // return parent.transform.Find(name).GetComponent<Button>();
        return parent.transform.Find(name)?.GetComponent<Button>();
    }

    public void ButtonClicked(int index)
    {
        this.index = index;
        List<FinancialData> financialList = FinancialManager.Instance.GetList();
        level = financialList[index].GetFinancialType();
        maxRedemptionAmount = financialList[index].GetTotalAmount();
        purchaseDate = financialList[index].GetPurchaseDate();
        if (ValidateRedeem(level, purchaseDate))
        {
            ShowText();
            dialog.SetActive(true);
        }
        else
        {
            ShowFailText();
            dialogFail.SetActive(true);
        }
    }

    public void ClickPartRedeemButton()
    {
        inputField.onValueChanged.AddListener(ValidateInput);
        validate = false;
        confirmButton.interactable = validate;
        redeemType = false;
    }

    public void ClickAllRedeemButton()
    {
        validate = true;
        confirmButton.interactable = validate;
        redeemType = true;
    }

    public void ValidateInput(string input)
    {
        if (float.TryParse(input, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float inputNumber))
        {
            if (inputNumber > maxRedemptionAmount)
            {
                warningText.text = "Your maximum redemption amount is " + maxRedemptionAmount;
                validate = false;
            }
            else if (input.Length == 0)
            {
                warningText.text = "";
                validate = false;
            }
            else
            {
                warningText.text = "";
                validate = true;
                redemptionAmount = inputNumber;
            }
        }

        confirmButton.interactable = validate;
    }

    private bool ValidateRedeem(string level, int purchaseDate)
    {
        switch (level)
        {
            case "R1":
                redeemAvailableDate = purchaseDate + 1;
                break;
            case "R3":
                redeemAvailableDate = purchaseDate + 3;
                break;
            case "R5":
                redeemAvailableDate = purchaseDate + 6;
                break;
        }

        if (redeemAvailableDate <= PlayerManager.Instance.GetRound())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ShowText()
    {
        levelText.text = "Level: " + level;
        amountText.text = "Maximum Redemption Amount: " + maxRedemptionAmount.ToString("F2");
    }

    private void ShowFailText()
    {
        failText.text = "The earliest redemption round for this product is " + redeemAvailableDate;
    }

    public void RedemptionSubmit()
    {
        if (redeemType)
        {
            FinancialManager.Instance.AddRedeemedProduct(FinancialManager.Instance.FindRedeemedProduct(index));
            FinancialManager.Instance.RemoveFinancial(index);
            PlayerManager.Instance.AddPlayerCash(maxRedemptionAmount);
            PlayerManager.Instance.SetBankFinanceValue();
        }
        else
        {
            PlayerManager.Instance.AddPlayerCash(redemptionAmount);
            FinancialManager.Instance.RedeemPartially(index, redemptionAmount);
            PlayerManager.Instance.SetBankFinanceValue();
        }
    }
}
