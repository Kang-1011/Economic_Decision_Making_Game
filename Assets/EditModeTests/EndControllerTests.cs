// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

public class EndControllerTests
{
    private PlayerManager playerManager;
    private EndController endController;
    private GameObject netAssetValueOb;
    private GameObject initialAssetValueOb;
    private GameObject profitLossValueOb;

    [SetUp]
    public void SetUp()
    {
        playerManager = new GameObject().AddComponent<PlayerManager>();
        playerManager.InitializePlayerAssets();

        endController = new GameObject().AddComponent<EndController>();
        endController.totalAsset = playerManager.GetPlayerCash() + playerManager.houseValue + playerManager.GetStockValue() + 
                                    playerManager.GetFuturesValue() + playerManager.GetBankDepositValue() + playerManager.GetBankFinanceValue();
        endController.netAsset = endController.totalAsset - playerManager.GetBankLoanValue();
        endController.profit = endController.netAsset - endController.initialAsset;
        endController.playerManager = playerManager;

        netAssetValueOb = new GameObject();
        endController.netAssetValue = netAssetValueOb.AddComponent<TextMeshProUGUI>();
        initialAssetValueOb = new GameObject();
        endController.initialAssetValue = initialAssetValueOb.AddComponent<TextMeshProUGUI>();
        profitLossValueOb = new GameObject();
        endController.profitLossValue = profitLossValueOb.AddComponent<TextMeshProUGUI>();
    }

    [Test]
    public void UpdateData()
    {
        endController.UpdateData();
        Assert.AreEqual("¥ " + endController.netAsset.ToString("F2"), endController.netAssetValue.text);
        Assert.AreEqual("¥ 1000000", endController.initialAssetValue.text);
        Assert.AreEqual("¥ " + endController.profit.ToString("F2"), endController.profitLossValue.text);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(playerManager.gameObject);
        Object.DestroyImmediate(endController.gameObject);
        Object.DestroyImmediate(netAssetValueOb);
        Object.DestroyImmediate(initialAssetValueOb);
        Object.DestroyImmediate(profitLossValueOb);
    }
}
