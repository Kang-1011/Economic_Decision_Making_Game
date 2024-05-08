// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

public class PurchaseFinancialTests
{
    private PurchaseFinancial purchaseFinancial;
    private PlayerManager playerManager;

    private GameObject inputFieldOb;
    private GameObject toggleOb;
    private GameObject levelOb;
    private GameObject warningTextOb;
    private GameObject buttonOb;

    private GameObject nameTextOb;
    private GameObject amountTextOb;
    private GameObject typeTextOb;

    [SetUp]
    public void SetUp()
    {
        GameObject playerManagerOb = new GameObject();
        playerManager = playerManagerOb.AddComponent<PlayerManager>();
        playerManager.InitializePlayerAssets();

        GameObject purchaseFinancialOb = new GameObject();
        purchaseFinancial = purchaseFinancialOb.AddComponent<PurchaseFinancial>();
        purchaseFinancial.cash = 100000.0f;

        inputFieldOb = new GameObject();
        purchaseFinancial.inputField = inputFieldOb.AddComponent<TMP_InputField>();
        toggleOb = new GameObject();
        purchaseFinancial.toggle = toggleOb.AddComponent<Toggle>();
        levelOb = new GameObject();
        purchaseFinancial.level = levelOb.AddComponent<TextMeshProUGUI>();
        warningTextOb = new GameObject();
        purchaseFinancial.warning = warningTextOb.AddComponent<TextMeshProUGUI>();
        buttonOb = new GameObject();
        purchaseFinancial.submitButton = buttonOb.AddComponent<Button>();
        
        nameTextOb = new GameObject();
        purchaseFinancial.productName = nameTextOb.AddComponent<TextMeshProUGUI>();
        amountTextOb = new GameObject();
        purchaseFinancial.amountText = amountTextOb.AddComponent<TextMeshProUGUI>();
        typeTextOb = new GameObject();
        purchaseFinancial.typeText = typeTextOb.AddComponent<TextMeshProUGUI>();
    }

    [Test]
    public void ValidateInput_CheckNoInput()
    {
        // inputNumber == null
        purchaseFinancial.ValidateInput("");
        Assert.IsFalse(purchaseFinancial.GetValidateAmount(), "Validate should be false for invalid input.");
        Assert.IsNull(purchaseFinancial.warning.text);
    }

    [Test]
    public void ValidateInput_CheckInvalidInput()
    {
        // inputNumber < miniAmount
        purchaseFinancial.miniAmount = 100f;
        purchaseFinancial.ValidateInput("1");
        Assert.IsFalse(purchaseFinancial.GetValidateAmount(), "Validate should be false for invalid input.");
        Assert.AreEqual("The minimum amount is " + purchaseFinancial.miniAmount + " yuan", purchaseFinancial.warning.text);

        // inputNumber > cash
        purchaseFinancial.ValidateInput("10000000000");
        Assert.IsFalse(purchaseFinancial.GetValidateAmount(), "Validate should be false for invalid input.");
        Assert.AreEqual("You don't have enough cash", purchaseFinancial.warning.text);
    
        // type == "R5" && netAsset < 1001000
        purchaseFinancial.type = "R5";
        purchaseFinancial.netAsset = 10000;
        purchaseFinancial.ValidateInput("5000");
        Assert.IsFalse(purchaseFinancial.GetValidate(), "Validate should be false for invalid input.");
        Assert.AreEqual("You can not purchase this product until you get a net asset of 1001000!", purchaseFinancial.warning.text);
    }

    [Test]
    public void ValidateInput_CheckValidInput()
    {
        // miniAmount <= inputNumber <= cash, netAsset > 1001000
        purchaseFinancial.type = "R5";
        purchaseFinancial.miniAmount = 5000f;
        purchaseFinancial.netAsset = 1005000;
        purchaseFinancial.ValidateInput("6000"); 
        Assert.IsTrue(purchaseFinancial.GetValidateAmount(), "Validate should be true for valid input.");
        Assert.AreEqual("", purchaseFinancial.warning.text);
    }

    [Test]
    public void ToggleValueChanged_TrueValidate()
    {   
        purchaseFinancial.type = "R3";
        purchaseFinancial.miniAmount = 1000f;
        purchaseFinancial.ValidateInput("5000"); 
        purchaseFinancial.toggle.isOn = true;

        purchaseFinancial.ToggleValueChanged(purchaseFinancial.toggle);
        Assert.IsTrue(purchaseFinancial.GetValidate());
        Assert.AreEqual("Amount: 5000", purchaseFinancial.amountText.text);
        Assert.AreEqual("Level: R3", purchaseFinancial.typeText.text);
    }

    [Test]
    public void ToggleValueChanged_FalseValidate()
    {   
        purchaseFinancial.type = "R3";
        purchaseFinancial.miniAmount = 1000f;
        purchaseFinancial.ValidateInput("5000"); 
        purchaseFinancial.toggle.isOn = false;

        purchaseFinancial.ToggleValueChanged(purchaseFinancial.toggle);
        Assert.IsTrue(purchaseFinancial.GetValidateAmount());
        Assert.IsFalse(purchaseFinancial.submitButton.interactable);
    }


    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(playerManager.gameObject);
        Object.DestroyImmediate(purchaseFinancial.gameObject);
        Object.DestroyImmediate(inputFieldOb);
        Object.DestroyImmediate(toggleOb);
        Object.DestroyImmediate(levelOb);
        Object.DestroyImmediate(warningTextOb);
        Object.DestroyImmediate(buttonOb);
        Object.DestroyImmediate(nameTextOb);
        Object.DestroyImmediate(amountTextOb);
        Object.DestroyImmediate(typeTextOb);
    }
}
