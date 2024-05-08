// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CurrentDepositManagerTests
{
    private CurrentDepositManager currentDepositManager;
    [SetUp]
    public void SetUp()
    {
        GameObject gameObject = new GameObject();
        currentDepositManager = gameObject.AddComponent<CurrentDepositManager>();
    }

    [Test]
    public void AddCurrentDeposit()
    {
        currentDepositManager.AddCurrentDeposit(1000f, 0.01f, 1);
        Assert.IsNotNull(currentDepositManager);
        Assert.AreEqual(1, currentDepositManager.depositDatas.Count);
    }

    [Test]
    public void RemoveCurrentDeposit()
    {
        CurrentDepositData currentDepositData = new CurrentDepositData(100f, 0.01f, 1);
        currentDepositManager.depositDatas.Add(currentDepositData);

        currentDepositManager.RemoveDeposit(currentDepositData);
        Assert.AreEqual(0, currentDepositManager.depositDatas.Count);
    }

    [Test]
    public void CalculateTotalAmount()
    {
        currentDepositManager.AddCurrentDeposit(1000f, 0.01f, 1);
        currentDepositManager.AddCurrentDeposit(500f, 0.01f, 2);
        Assert.AreEqual(1500f, currentDepositManager.CalculateTotalAmount());
    }

    [Test]
    public void AddWithdrawalCurrentDeposit()
    {
        Assert.AreEqual(0, currentDepositManager.withdrawDatas.Count);

        currentDepositManager.AddWithdrawalCurrentDeposit(new CurrentDepositData(1000f, 0.01f, 1));
        currentDepositManager.AddWithdrawalCurrentDeposit(new CurrentDepositData(1000f, 0.01f, 4));
        Assert.AreEqual(2, currentDepositManager.withdrawDatas.Count);
    }

    [Test]
    public void CalculateTotalPrincipal()
    {
        currentDepositManager.AddCurrentDeposit(1000f, 0.01f, 1);
        currentDepositManager.AddWithdrawalCurrentDeposit(new CurrentDepositData(1500f, 0.01f, 1));
        Assert.AreEqual(2500f, currentDepositManager.CalculateTotalPrincipal());
    }

    [Test]
    public void CalculateTotalRevenue()
    {
        currentDepositManager.AddCurrentDeposit(1000f, 0.01f, 1);
        currentDepositManager.AddWithdrawalCurrentDeposit(new CurrentDepositData(1500f, 0.01f, 1));
        Assert.AreEqual(2500f, currentDepositManager.CalculateTotalRevenue());
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(currentDepositManager.gameObject);
    }
}
