// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class LoanManagerTests : MonoBehaviour
{
    private LoanManager loanManager;
    private PlayerManager playerManager;
    [SetUp]
    public void SetUp()
    {
        GameObject gameObject = new GameObject();
        loanManager = gameObject.AddComponent<LoanManager>();
        loanManager.AddLoan(4000, 5, 12, 1);
        loanManager.AddLoan(50000, 6, 24, 1);
        loanManager.AddLoan(12000, 7, 36, 1);
        loanManager.AddLoan(7000, 5, 12, 1);
    }

    [Test]
    public void AddFinancial()
    {
        loanManager.AddLoan(8000, 5, 12, 1);
        Assert.AreEqual(5, loanManager.GetList().Count);
    }

    [Test]
    public void RemoveLoan()
    {
        int initialCount = loanManager.GetList().Count;
        loanManager.RemoveLoan(0);
        Assert.AreEqual(initialCount - 1, loanManager.GetList().Count);
    }

    [Test]
    public void AddRepayedRound()
    {
        loanManager.AddRepayedRound(0);
        List<LoanData> loanDatas = loanManager.GetList();
        Assert.AreEqual(1, loanDatas[0].GetRepayedRound());
    }

    [Test]
    public void FinishRepay()
    {
        int index = 0;
        loanManager.AddRepayedRound(index);
        loanManager.FinishRepay(index);
        bool isRepayed = loanManager.GetList()[index].GetIsRepayed();
        Assert.IsTrue(isRepayed);
    }

    [Test]
    public void CalculateTotalLoan()
    {
        float totalLoan = loanManager.CalculateTotalLoan();

        float expectedTotalLoan = 0f;
        foreach (LoanData loanData in loanManager.GetList())
        {
            expectedTotalLoan += loanData.GetRemainAmount();
        }
        Assert.AreEqual(expectedTotalLoan, totalLoan);
    }

    [Test]
    public void CalculateTotalLoanAmount()
    {
        float totalLoanAmount = loanManager.CalculateTotalLoanAmount();

        float expectedTotalLoanAmount = 0f;
        foreach (LoanData loanData in loanManager.GetList())
        {
            expectedTotalLoanAmount += loanData.GetLoanAmount();
        }
        Assert.AreEqual(expectedTotalLoanAmount, totalLoanAmount);
    }

    [TearDown]
    public void Teardown()
    {
        if (loanManager.gameObject != null)
        {
            GameObject.DestroyImmediate(loanManager.gameObject);
        }
    }
}
