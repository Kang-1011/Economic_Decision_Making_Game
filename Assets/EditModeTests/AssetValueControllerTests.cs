// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

public class AssetValueControllerTests
{
    private PlayerManager playerManager;
    private AssetValueController assetValueController;

    private GameObject totalAssetOb;
    private GameObject liabilitiesTextOb;
    private GameObject availableCashOb;
    private GameObject netAssetValueOb;
    private GameObject monthlyRentIncomeTextOb;
    private GameObject monthlyPaymentTextOb;
    
    [SetUp]
    public void SetUp()
    {
        playerManager = new GameObject().AddComponent<PlayerManager>();
        playerManager.InitializePlayerAssets();

        assetValueController = new GameObject().AddComponent<AssetValueController>();
        assetValueController.total = playerManager.GetPlayerCash() + playerManager.GetBankDepositValue() + playerManager.GetBankFinanceValue() + playerManager.GetFuturesValue() + playerManager.GetStockValue() +playerManager.houseValue;
        assetValueController.liabilities = playerManager.houseLoan + playerManager.GetBankLoanValue();
        assetValueController.netAsset = assetValueController.total - assetValueController.liabilities;
        assetValueController.playerManager = playerManager;

        totalAssetOb = new GameObject();
        assetValueController.totalAsset = totalAssetOb.AddComponent<TextMeshProUGUI>();
        liabilitiesTextOb = new GameObject();
        assetValueController.liabilitiesText = liabilitiesTextOb.AddComponent<TextMeshProUGUI>();
        availableCashOb = new GameObject();
        assetValueController.availableCash = availableCashOb.AddComponent<TextMeshProUGUI>();
        netAssetValueOb = new GameObject();
        assetValueController.netAssetValue = netAssetValueOb.AddComponent<TextMeshProUGUI>();
        monthlyRentIncomeTextOb = new GameObject();
        assetValueController.monthlyRentIncomeText = monthlyRentIncomeTextOb.AddComponent<Text>();
        monthlyPaymentTextOb = new GameObject();
        assetValueController.monthlyPaymentText = monthlyPaymentTextOb.AddComponent<Text>();
    }

    [Test]
    public void UpdateData()
    {
        assetValueController.UpdateData();
        Assert.AreEqual("¥ " + assetValueController.total.ToString("F0"), assetValueController.totalAsset.text);
        Assert.AreEqual("¥ " + playerManager.GetPlayerCash().ToString("F0"), assetValueController.availableCash.text);
        Assert.AreEqual("¥ " + assetValueController.liabilities.ToString("F0"), assetValueController.liabilitiesText.text);
        Assert.AreEqual("¥ " + assetValueController.netAsset.ToString("F0"), assetValueController.netAssetValue.text);

        Assert.AreEqual(playerManager.monthlyRentIncome.ToString("F0"), assetValueController.monthlyRentIncomeText.text);
        Assert.AreEqual(playerManager.monthlyPayment.ToString("F0"), assetValueController.monthlyPaymentText.text);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(playerManager.gameObject);
        Object.DestroyImmediate(assetValueController.gameObject);
        Object.DestroyImmediate(totalAssetOb);
        Object.DestroyImmediate(liabilitiesTextOb);
        Object.DestroyImmediate(availableCashOb);
        Object.DestroyImmediate(netAssetValueOb);
        Object.DestroyImmediate(monthlyRentIncomeTextOb);
        Object.DestroyImmediate(monthlyPaymentTextOb);
    }
}
