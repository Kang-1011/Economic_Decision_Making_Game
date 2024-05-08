// All code was written by the team

using System;
using UnityEngine;
using UnityEngine.UI;

public class FuturesTrade : MonoBehaviour
{
    public InputField PriceInputField;
    public InputField QuantityInputField;
    public InputField TotalValueInputField;
    public InputField StopLossInputField;
    public InputField TakeProfitInputField;

    private void Start()
    {
        // Add listener methods to the price and quantity input fields
        QuantityInputField.onValueChanged.AddListener(ValidateInput);

        QuantityInputField.onValueChanged.AddListener(UpdateTotal);
    }
    
    // Update the content of 'TotalValueInputField' based on the price and quantity
    public void UpdateTotal(string value)
    {
        double price = double.Parse(PriceInputField.text);

        // Stop producing error when the quantity field is empty
        if (value == "")
            return;

        double quantity = double.Parse(QuantityInputField.text);

        double totalValue = price * quantity;
        totalValue = Math.Round(totalValue * 100f) / 100f;

        TotalValueInputField.text = totalValue.ToString("0.00");
    }

    public void ValidateInput(string value)
    {
        // Try parsing the input value as an integer
        if (!int.TryParse(value, out int intValue) || intValue <= 0)
        {
            // If parsing fails, reset the input field to its previous valid value
            QuantityInputField.text = "";
        }
    }
}
