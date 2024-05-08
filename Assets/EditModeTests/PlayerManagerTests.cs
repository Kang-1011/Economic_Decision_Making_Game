// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerManagerTests
{
    private PlayerManager playerManager;
    [SetUp]
    public void SetUp()
    {
        playerManager = new GameObject().AddComponent<PlayerManager>();
        playerManager.InitializePlayerAssets();
    }

    [Test]
    public void InitializePlayerAssets()
    {
        Assert.AreEqual(1, playerManager.GetRound());
        Assert.AreEqual(1000000.0f, playerManager.GetPlayerCash());
        Assert.AreEqual(0.0f, playerManager.GetStockValue());
        Assert.AreEqual(0.0f, playerManager.GetFuturesValue());
        Assert.AreEqual(0.0f, playerManager.GetBankDepositValue());
        Assert.AreEqual(0.0f, playerManager.GetBankFinanceValue());
        Assert.AreEqual(0.0f, playerManager.GetBankLoanValue());
        Assert.AreEqual(0.0f, playerManager.monthlyPayment);
    }

    [Test]
    public void AddRound()
    {
        playerManager.AddRound();
        Assert.AreEqual(2, playerManager.GetRound());
    }

    [Test]
    public void AddPlayerCash()
    {
        playerManager.AddPlayerCash(1000f);
        Assert.AreEqual(1001000.0f, playerManager.GetPlayerCash());
    }

    [Test]
    public void SubtractPlayerCash()
    {
        playerManager.SubtractPlayerCash(1000f);
        Assert.AreEqual(999000.0f, playerManager.GetPlayerCash());
    }

    [Test]
    public void SetStockValue()
    {
        playerManager.SetStockValue(1000f);
        Assert.AreEqual(1000f, playerManager.GetStockValue());
    }

    [Test]
    public void SetFuturesValue()
    {
        playerManager.SetFuturesValue(2000f);
        Assert.AreEqual(2000f, playerManager.GetFuturesValue());
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(playerManager.gameObject);
    }
}
