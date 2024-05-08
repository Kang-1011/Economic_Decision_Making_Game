using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DepositManagerTests
{
    private DepositManager depositManager;
    [SetUp]
    public void SetUp()
    {
        GameObject gameObject = new GameObject();
        depositManager = gameObject.AddComponent<DepositManager>();
    }

    [Test]
    public void AddTimeDeposit()
    {
        depositManager.AddDeposit(1000f, 0.013f, 3, 1);
        Assert.IsNotNull(depositManager);
        Assert.AreEqual(1, depositManager.depositDatas.Count);
    }

    [Test]
    public void RemoveTimeDeposit()
    {
        DepositData depositData = new DepositData(1000f, 0.025f, 24, 1);
        depositManager.depositDatas.Add(depositData);

        depositManager.RemoveDeposit(depositData);
        Assert.AreEqual(0, depositManager.depositDatas.Count);
    }

    [Test]
    public void CalculateTotalAmount()
    {
        depositManager.AddDeposit(1000f, 0.0255f, 24, 1);
        depositManager.AddDeposit(1500f, 0.0275f, 36, 2);
        Assert.AreEqual(2500f, depositManager.CalculateTotalAmount());
    }

    [Test]
    public void AddWithdrawalDeposit()
    {
        Assert.AreEqual(0, depositManager.withdrawDatas.Count);

        depositManager.AddWithdrawalDeposit(new DepositData(1000f, 0.0175f, 3, 1));
        depositManager.AddWithdrawalDeposit(new DepositData(5000f, 0.0275f, 36, 1));
        Assert.AreEqual(2, depositManager.withdrawDatas.Count);
    }

    [Test]
    public void CalculateTotalPrincipal()
    {
        depositManager.AddDeposit(1000f, 0.0175f, 3, 1);
        depositManager.AddWithdrawalDeposit(new DepositData(5000f, 0.0275f, 36, 1));
        Assert.AreEqual(6000f, depositManager.CalculateTotalPrincipal());
    }

    [Test]
    public void CalculateTotalRevenue()
    {
        depositManager.AddDeposit(1000f, 0.0175f, 3, 1);
        depositManager.AddDeposit(1500f, 0.0255f, 24, 2);
        depositManager.AddWithdrawalDeposit(new DepositData(5000f, 0.0275f, 36, 6));
        Assert.AreEqual(7500f, depositManager.CalculateTotalPrincipal());
    }


    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(depositManager.gameObject);
    }
}

