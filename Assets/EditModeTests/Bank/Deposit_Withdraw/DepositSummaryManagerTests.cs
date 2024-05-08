// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

public class DepositSummaryManagerTests
{
    private DepositSummaryManager depositSummaryManager;
    private DepositManager depositManager;
    private CurrentDepositManager currentDepositManager;
    private TextMeshProUGUI SummaryText;
    private Text totalPrincipalText;
    private Text totalIncomeText;


    [SetUp]
    public void SetUp()
    {
        depositSummaryManager = new GameObject().AddComponent<DepositSummaryManager>();
        
        depositManager = new GameObject().AddComponent<DepositManager>();
        depositSummaryManager.timeDepositItem = depositManager.depositDatas;
        depositSummaryManager.timeWithdrawalItem = depositManager.withdrawDatas;

        currentDepositManager = new GameObject().AddComponent<CurrentDepositManager>();
        depositSummaryManager.currentDepositItem = currentDepositManager.depositDatas;
        depositSummaryManager.currentWithdrawalItem = currentDepositManager.withdrawDatas;

        SummaryText = new GameObject().AddComponent<TextMeshProUGUI>();
        depositSummaryManager.SummaryText = SummaryText;
        totalPrincipalText = new GameObject().AddComponent<Text>();
        depositSummaryManager.totalPrincipalText = totalPrincipalText;
        totalIncomeText = new GameObject().AddComponent<Text>();
        depositSummaryManager.totalIncomeText = totalIncomeText;

    }

    [Test]
    public void DisplayDepositSummary_NoAnyDeposit()
    {
        depositSummaryManager.totalPrincipal = 0f;
        depositSummaryManager.totalRevenue = 0f;
        depositSummaryManager.DisplayDepositSummary();

        Assert.AreEqual("￥ 0.00", depositSummaryManager.totalPrincipalText.text);
        Assert.AreEqual("￥ 0.00", depositSummaryManager.totalIncomeText.text);
        Assert.AreEqual("You have not deposited any cash in the deposit module.\n"+
                        "\nCurrent deposits and time deposits, as principal-guranteed finanicial products, " + 
                        "carry very low risk and offer stable income.Compared to current deposits, " + 
                        "time deposits provide higher interest rates, but the flexibility of the funds is lower. " +
                        "Both are suitable for investors with a low risk tolerance.", depositSummaryManager.SummaryText.text);
    }

    [Test]
    public void DisplayDepositSummary_HasCurrentDeposit()
    {
        depositSummaryManager.totalPrincipal = 1000f;
        depositSummaryManager.totalRevenue = 10f;
        currentDepositManager.AddCurrentDeposit(1000f, 0.0035f, 1);
        depositSummaryManager.DisplayDepositSummary();

        Assert.AreEqual("￥ 1000.00", depositSummaryManager.totalPrincipalText.text);
        Assert.AreEqual("￥ 10.00", depositSummaryManager.totalIncomeText.text);
        Assert.AreEqual("You have chosen to make current deposits.\n"+
                        "\nCurrent deposits and time deposits, as principal-guranteed finanicial products, " + 
                        "carry very low risk and offer stable income.Compared to current deposits, " + 
                        "time deposits provide higher interest rates, but the flexibility of the funds is lower. " +
                        "Both are suitable for investors with a low risk tolerance.", depositSummaryManager.SummaryText.text);
    }

    [Test]
    public void DisplayDepositSummary_HasTimeDeposit()
    {
        depositSummaryManager.totalPrincipal = 1000f;
        depositSummaryManager.totalRevenue = 10f;
        depositManager.AddDeposit(1000f, 0.0135f, 3, 1);
        depositSummaryManager.DisplayDepositSummary();

        Assert.AreEqual("￥ 1000.00", depositSummaryManager.totalPrincipalText.text);
        Assert.AreEqual("￥ 10.00", depositSummaryManager.totalIncomeText.text);
        Assert.AreEqual("You have chosen to make time deposits.\n"+
                        "\nCurrent deposits and time deposits, as principal-guranteed finanicial products, " + 
                        "carry very low risk and offer stable income.Compared to current deposits, " + 
                        "time deposits provide higher interest rates, but the flexibility of the funds is lower. " +
                        "Both are suitable for investors with a low risk tolerance.", depositSummaryManager.SummaryText.text);
    }

    [Test]
    public void DisplayDepositSummary_HasTwoTypes()
    {
        depositSummaryManager.totalPrincipal = 1000f;
        depositSummaryManager.totalRevenue = 10f;
        currentDepositManager.AddCurrentDeposit(1000f, 0.0035f, 1);
        depositManager.AddDeposit(1000f, 0.0135f, 3, 1);
        depositSummaryManager.DisplayDepositSummary();

        Assert.AreEqual("￥ 1000.00", depositSummaryManager.totalPrincipalText.text);
        Assert.AreEqual("￥ 10.00", depositSummaryManager.totalIncomeText.text);
        Assert.AreEqual("You have chosen to make current deposits and time deposits.\n"+
                        "\nCurrent deposits and time deposits, as principal-guranteed finanicial products, " + 
                        "carry very low risk and offer stable income.Compared to current deposits, " + 
                        "time deposits provide higher interest rates, but the flexibility of the funds is lower. " +
                        "Both are suitable for investors with a low risk tolerance.", depositSummaryManager.SummaryText.text);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(depositSummaryManager.gameObject);
        Object.DestroyImmediate(SummaryText.gameObject);
        Object.DestroyImmediate(totalPrincipalText.gameObject);
        Object.DestroyImmediate(totalIncomeText.gameObject);
    }
}
