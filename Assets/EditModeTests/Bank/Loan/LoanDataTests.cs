// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LoanDataTests
{
    private LoanData loanData;
    private PlayerManager playerManager;
    private LoanManager loanManager;


    [SetUp]
    public void Setup()
    {
        loanData = new LoanData(10000, 0.05f, 12, 3);
        
        GameObject playerManagerOb = new GameObject();
        playerManager = playerManagerOb.AddComponent<PlayerManager>();
        playerManager.InitializePlayerAssets();

        GameObject loanManagerOb = new GameObject();
        loanManager = loanManagerOb.AddComponent<LoanManager>();

    }

    [Test]
    public void LoanData_CreateConstructor()
    {
        Assert.AreEqual(10000, loanData.GetLoanAmount());
        Assert.AreEqual(0.05f, loanData.GetRate());
        Assert.AreEqual(12, loanData.GetPeriod());
        Assert.AreEqual(3, loanData.GetLoanDate());
        Assert.AreEqual(15, loanData.GetFinishDate());
        Assert.AreEqual(10500f, loanData.GetRepayAmount());
        Assert.AreEqual(10500f, loanData.GetRemainAmount());
        Assert.AreEqual(10500f / 12, loanData.GetMonthlyRepayAmount());
        Assert.AreEqual(0, loanData.GetRepayedRound());
        Assert.AreEqual(0, loanData.GetDefaultRound());
        Assert.AreEqual(0f, loanData.GetDefaultAmount());
    }

    [Test]
    public void UpdateRemainAmount()
    {
        loanData.UpdateRemainAmount();
        Assert.AreEqual(10500f / 12 * 11, loanData.GetRemainAmount());
    }

    [Test]
    public void AddRepayedRound()
    {
        loanData.AddRepayedRound();
        Assert.AreEqual(1, loanData.GetRepayedRound());
    }

    [Test]
    public void AddDefaultRound()
    {
        loanData.AddDefaultRound();
        Assert.AreEqual(1, loanData.GetDefaultRound());
    }

    [Test]
    public void FinishRepay()
    {
        loanData.AddRepayedRound();
        loanData.FinishRepay();

        Assert.IsTrue(loanData.GetIsRepayed());
    }

    [TearDown]
    public void Teardown()
    {
        if (playerManager.gameObject != null)
        {
            GameObject.DestroyImmediate(playerManager.gameObject);
        }

        if (loanManager.gameObject != null)
        {
            GameObject.DestroyImmediate(loanManager.gameObject);
        }
    }
}
