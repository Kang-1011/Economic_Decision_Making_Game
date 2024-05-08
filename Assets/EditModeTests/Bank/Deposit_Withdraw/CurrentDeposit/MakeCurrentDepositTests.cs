// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using TMPro;

public class MakeCurrentDepositTests
{
    private MakeCurrentDeposit makeCurrentDeposit;
    private PlayerManager playerManager;
    private BankRateManager bankRateManager;
    private GameObject inputFieldOb;
    private GameObject warningTextOb;
    private GameObject buttonOb;
    private GameObject amountTextOb;
    private GameObject rateTextOb;

    [SetUp]
    public void SetUp()
    {
        GameObject playerManagerOb = new GameObject();
        playerManager = playerManagerOb.AddComponent<PlayerManager>();
        playerManager.InitializePlayerAssets();

        GameObject bankRateManagerOb = new GameObject();
        bankRateManager = bankRateManagerOb.AddComponent<BankRateManager>();
        bankRateManager.InitializeBankRates();

        GameObject makeCurrentDepositOb = new GameObject();
        makeCurrentDeposit = makeCurrentDepositOb.AddComponent<MakeCurrentDeposit>();
        makeCurrentDeposit.cash = 1000000.0f;
        makeCurrentDeposit.rate = bankRateManager.GetDepositRate_Current();

        inputFieldOb = new GameObject();
        makeCurrentDeposit.inputField = inputFieldOb.AddComponent<TMP_InputField>();
        warningTextOb = new GameObject();
        makeCurrentDeposit.warning = warningTextOb.AddComponent<TextMeshProUGUI>();
        buttonOb = new GameObject();
        makeCurrentDeposit.submitButton = buttonOb.AddComponent<Button>();

        amountTextOb = new GameObject();
        makeCurrentDeposit.amountText = amountTextOb.AddComponent<TextMeshProUGUI>();
        rateTextOb = new GameObject();
        makeCurrentDeposit.rateText = rateTextOb.AddComponent<TextMeshProUGUI>();
    }

    [Test]
    public void ValidateInput_CheckNoInput()
    {
        // inputNumber == null
        makeCurrentDeposit.ValidateInput("");
        Assert.IsFalse(makeCurrentDeposit.GetValidate(), "Validate should be false for invalid input.");
        Assert.IsNull(makeCurrentDeposit.warning.text);
    }

    [Test]
    public void ValidateInput_CheckInvalidInput()
    {
        // inputNumber < 50f
        makeCurrentDeposit.ValidateInput("1");
        Assert.IsFalse(makeCurrentDeposit.GetValidate(), "Validate should be false for invalid input.");
        Assert.AreEqual("The minimum deposit is 50 yuan", makeCurrentDeposit.warning.text);

        // inputNumber > cash
        makeCurrentDeposit.ValidateInput("10000000000");
        Assert.IsFalse(makeCurrentDeposit.GetValidate(), "Validate should be false for invalid input.");
        Assert.AreEqual("You don't have enough cash", makeCurrentDeposit.warning.text);
    }

    [Test]
    public void ValidateInput_CheckValidInput()
    {
        // 50f <= inputNumber <= cash
        makeCurrentDeposit.ValidateInput("100"); 
        Assert.IsTrue(makeCurrentDeposit.GetValidate(), "Validate should be true for valid input.");
        Assert.AreEqual("", makeCurrentDeposit.warning.text);
    }

    [Test]
    public void ShowInfo_UpdateText()
    {
        makeCurrentDeposit.ValidateInput("100"); 
        Assert.AreEqual("Amount of the deposit: 100", makeCurrentDeposit.amountText.text);
        Assert.AreEqual("Annual Interest Rate: " + (bankRateManager.GetDepositRate_Current()* 100).ToString() + "%", makeCurrentDeposit.rateText.text);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(makeCurrentDeposit.gameObject);
        Object.DestroyImmediate(playerManager.gameObject);
        Object.DestroyImmediate(bankRateManager.gameObject);
        Object.DestroyImmediate(inputFieldOb);
        Object.DestroyImmediate(warningTextOb);
        Object.DestroyImmediate(buttonOb);
        Object.DestroyImmediate(amountTextOb);
        Object.DestroyImmediate(rateTextOb);
    }
}
