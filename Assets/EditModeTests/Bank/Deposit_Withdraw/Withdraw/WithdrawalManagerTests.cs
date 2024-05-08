// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

public class WithdrawalManagerTests
{
    private DepositManager timeManager;
    private CurrentDepositManager currentManager;
    private PlayerManager playerManager;
    private WithdrawalManager withdrawalManager;

    private TextMeshProUGUI noTimeDeposit;
    private TextMeshProUGUI noCurrentDeposit;
    private GameObject TimeItemPrefabOb;
    private GameObject CurrentItemPrefabOb;
    private Transform Timecontent;
    private Transform Currentcontent;
    private Button toTimeButton;
    private Button toCurrentButton;
    private Transform CurrentPart;
    private Transform TimePart;

    private GameObject DialogOb;
    private GameObject ToastOb;

    [SetUp]
    public void SetUp()
    {
        playerManager = new GameObject().AddComponent<PlayerManager>();
        playerManager.InitializePlayerAssets();
        withdrawalManager = new GameObject().AddComponent<WithdrawalManager>();
        withdrawalManager.currentDepositRate = 0.0035f;
        withdrawalManager.currentRound = 1;

        timeManager = new GameObject().AddComponent<DepositManager>();
        withdrawalManager.Deposits = timeManager.depositDatas;
        currentManager = new GameObject().AddComponent<CurrentDepositManager>();
        withdrawalManager.CurrentDeposits = currentManager.depositDatas;

        noTimeDeposit = new GameObject().AddComponent<TextMeshProUGUI>();
        withdrawalManager.noTimeDeposit = noTimeDeposit;
        noCurrentDeposit = new GameObject().AddComponent<TextMeshProUGUI>();
        withdrawalManager.noCurrentDeposit = noCurrentDeposit;
        Timecontent = new GameObject("TimeContent").transform;
        withdrawalManager.Timecontent = Timecontent;
        Currentcontent = new GameObject("CurrentContent").transform;
        withdrawalManager.Currentcontent = Currentcontent;
        toTimeButton = new GameObject().AddComponent<Button>();
        withdrawalManager.toTimeButton = toTimeButton;
        toCurrentButton = new GameObject().AddComponent<Button>();
        withdrawalManager.toCurrentButton = toCurrentButton;
        CurrentPart = new GameObject("CurrentPart").transform;
        withdrawalManager.CurrentPart = CurrentPart;
        TimePart = new GameObject("TimePart").transform;
        withdrawalManager.TimePart = TimePart;

        // Create a TimeDepositItem Prefab
        TimeItemPrefabOb = new GameObject();
        withdrawalManager.TimeItemPrefab = TimeItemPrefabOb;
        CreateTextComponent("Principal", withdrawalManager.TimeItemPrefab);
        CreateTextComponent("Rate", withdrawalManager.TimeItemPrefab);
        CreateTextComponent("Period", withdrawalManager.TimeItemPrefab);
        CreateTextComponent("DepositDate", withdrawalManager.TimeItemPrefab);
        CreateTextComponent("FinishDate", withdrawalManager.TimeItemPrefab);
        GameObject buttonObject1 = new GameObject("Button");
        buttonObject1.AddComponent<Button>();
        buttonObject1.transform.SetParent(withdrawalManager.TimeItemPrefab.transform, false);
        
        // Create a CurrentDepositItem Prefab
        CurrentItemPrefabOb = new GameObject();
        withdrawalManager.CurrentItemPrefab = CurrentItemPrefabOb;
        CreateTextComponent("Principal", withdrawalManager.CurrentItemPrefab);
        CreateTextComponent("Rate", withdrawalManager.CurrentItemPrefab);
        CreateTextComponent("DepositDate", withdrawalManager.CurrentItemPrefab);
        GameObject buttonObject2 = new GameObject("Button");
        buttonObject2.AddComponent<Button>();
        buttonObject2.transform.SetParent(withdrawalManager.CurrentItemPrefab.transform, false);

        // Create a Withdraw Dialog Prefab
        DialogOb = new GameObject("Dialog");
        withdrawalManager.Dialog = DialogOb;
        GameObject Window = new GameObject("Window");
        Window.transform.SetParent(withdrawalManager.Dialog.transform, false);
        GameObject Text = new GameObject("Text");
        Text.transform.SetParent(Window.transform, false);
        CreateTextComponent("Warning", Text);
        CreateTextComponent("Principal", Text);
        CreateTextComponent("DepositDate", Text);
        CreateTextComponent("FinishDate", Text);
        CreateTextComponent("WithdrawDate", Text);
        CreateTextComponent("Rate", Text);
        CreateTextComponent("Period", Text);
        CreateTextComponent("Interest", Text);
        CreateTextComponent("Amount", Text);
        GameObject confirmButton = new GameObject("ConfirmButton");
        confirmButton.AddComponent<Button>();
        confirmButton.transform.SetParent(Window.transform, false);
    }

    [Test]
    public void UpdateTimeDepositItemUI()
    {
        DepositData data = new DepositData(1000f, 0.0135f, 12, 1);
        withdrawalManager.UpdateTimeDepositItemUI(withdrawalManager.TimeItemPrefab, data);

        TextMeshProUGUI principalText = FindTextComponentByName(withdrawalManager.TimeItemPrefab, "Principal");
        TextMeshProUGUI rateText = FindTextComponentByName(withdrawalManager.TimeItemPrefab, "Rate");
        TextMeshProUGUI periodText = FindTextComponentByName(withdrawalManager.TimeItemPrefab, "Period");
        TextMeshProUGUI depositDateText = FindTextComponentByName(withdrawalManager.TimeItemPrefab, "DepositDate");
        TextMeshProUGUI finishDateText = FindTextComponentByName(withdrawalManager.TimeItemPrefab, "FinishDate");

        Assert.AreEqual(data.GetPrincipal().ToString(), principalText.text);
        Assert.AreEqual(data.GetRate() * 100 + "%", rateText.text);
        Assert.AreEqual(data.GetPeriod() + "\nRounds", periodText.text);
        Assert.AreEqual("Round\n" + data.GetDepositDate(), depositDateText.text);
        Assert.AreEqual("Round\n" + data.GetFinishDate(), finishDateText.text);
        Assert.IsNotNull(withdrawalManager.TimeItemPrefab.transform.Find("Button").GetComponent<Button>());
    }

    [Test]
    public void UpdateCurrentDepositItemUI()
    {
        CurrentDepositData data = new CurrentDepositData(1000f, 0.0135f, 1);
        withdrawalManager.UpdateCurrentDepositItemUI(withdrawalManager.CurrentItemPrefab, data);

        TextMeshProUGUI principalText = FindTextComponentByName(withdrawalManager.CurrentItemPrefab, "Principal");
        TextMeshProUGUI rateText = FindTextComponentByName(withdrawalManager.CurrentItemPrefab, "Rate");
        TextMeshProUGUI depositDateText = FindTextComponentByName(withdrawalManager.CurrentItemPrefab, "DepositDate");

        Assert.AreEqual(data.GetPrincipal().ToString(), principalText.text);
        Assert.AreEqual(data.GetRate() * 100 + "%", rateText.text);
        Assert.AreEqual("Round " + data.GetDepositDate(), depositDateText.text);
        Assert.IsNotNull(withdrawalManager.CurrentItemPrefab.transform.Find("Button").GetComponent<Button>());
    }

    [Test]
    public void ShowTimeDepositItem_NoDeposit()
    {
        withdrawalManager.Deposits.Clear();
        withdrawalManager.ShowTimeDepositItem();
        Assert.IsTrue(withdrawalManager.noTimeDeposit.gameObject.activeSelf);
    }

    [Test]
    public void ShowTimeDepositItem_HasDeposits_CheckTowListsCount()
    {
        timeManager.AddDeposit(1000f, 0.0135f, 3, 1);
        timeManager.AddDeposit(1500f, 0.0225f, 24, 5);

        withdrawalManager.ShowTimeDepositItem();
        Assert.AreEqual(withdrawalManager.Deposits.Count, withdrawalManager.Timecontent.childCount);
    }

    [Test]
    public void ShowTimeDepositItem_HasDeposits_CheckUI()
    {
        timeManager.AddDeposit(1000f, 0.0135f, 3, 1);
        timeManager.AddDeposit(1000f, 0.0135f, 3, 1);
        timeManager.AddDeposit(1500f, 0.0225f, 24, 5);
        
        withdrawalManager.ShowTimeDepositItem();
        Assert.IsFalse(withdrawalManager.noTimeDeposit.gameObject.activeSelf);
        for (int i = 0; i < withdrawalManager.Timecontent.childCount; i++)
        {
            Transform child = withdrawalManager.Timecontent.GetChild(i);
            Assert.IsTrue(child.gameObject.activeSelf);
        }
        
        GameObject uiItem = withdrawalManager.Timecontent.GetChild(2).gameObject;
        TextMeshProUGUI principalText = FindTextComponentByName(uiItem, "Principal");
        TextMeshProUGUI rateText = FindTextComponentByName(uiItem, "Rate");
        TextMeshProUGUI periodText = FindTextComponentByName(uiItem, "Period");
        TextMeshProUGUI depositDateText = FindTextComponentByName(uiItem, "DepositDate");
        TextMeshProUGUI finishDateText = FindTextComponentByName(uiItem, "FinishDate");

        Assert.AreEqual("1500", principalText.text);
        Assert.AreEqual(0.0225f * 100 + "%", rateText.text);
        Assert.AreEqual("24" + "\nRounds", periodText.text);
        Assert.AreEqual("Round\n" + 5, depositDateText.text);
        Assert.AreEqual("Round\n" + 29, finishDateText.text);
        Assert.IsNotNull(uiItem.transform.Find("Button").GetComponent<Button>());
    }

    [Test]
    public void ShowCurrentDepositItem_NoDeposit()
    {
        withdrawalManager.CurrentDeposits.Clear();
        withdrawalManager.ShowCurrentDepositItem();
        Assert.IsTrue(withdrawalManager.noCurrentDeposit.gameObject.activeSelf);
    }

    [Test]
    public void ShowCurrentDepositItem_HasDeposits_CheckTowListsCount()
    {
        currentManager.AddCurrentDeposit(1000f, 0.0035f, 1);
        currentManager.AddCurrentDeposit(1500f, 0.0035f, 2);

        withdrawalManager.ShowCurrentDepositItem();
        Assert.AreEqual(withdrawalManager.CurrentDeposits.Count, withdrawalManager.Currentcontent.childCount);
    }

    [Test]
    public void ShowCurrentDepositItem_HasDeposits_CheckUI()
    {
        currentManager.AddCurrentDeposit(1000f, 0.0035f, 1);
        currentManager.AddCurrentDeposit(1000f, 0.0035f, 12);
        currentManager.AddCurrentDeposit(3000f, 0.0035f, 24);

        withdrawalManager.ShowCurrentDepositItem();
        Assert.IsFalse(withdrawalManager.noCurrentDeposit.gameObject.activeSelf);
        for (int i = 0; i < withdrawalManager.Currentcontent.childCount; i++)
        {
            Transform child = withdrawalManager.Currentcontent.GetChild(i);
            Assert.IsTrue(child.gameObject.activeSelf);
        }
        
        GameObject uiItem = withdrawalManager.Currentcontent.GetChild(2).gameObject;
        TextMeshProUGUI principalText = FindTextComponentByName(uiItem, "Principal");
        TextMeshProUGUI rateText = FindTextComponentByName(uiItem, "Rate");
        TextMeshProUGUI depositDateText = FindTextComponentByName(uiItem, "DepositDate");

        Assert.AreEqual("3000", principalText.text);
        Assert.AreEqual(0.0035f * 100 + "%", rateText.text);
        Assert.AreEqual("Round " + 24, depositDateText.text);
        Assert.IsNotNull(uiItem.transform.Find("Button").GetComponent<Button>());
    }

    [Test]
    public void ShowDialog_CurrentDepositDialog()
    {
        CurrentDepositData data = new CurrentDepositData(1000f, 0.0035f, 1);

        withdrawalManager.ShowDialog(data);
        Assert.AreEqual(1, withdrawalManager.CurrentPart.childCount);

        GameObject dialog = withdrawalManager.CurrentPart.GetChild(0).gameObject;
        TextMeshProUGUI principalText = FindTextComponentByName(dialog, "Principal");
        TextMeshProUGUI depositDate = FindTextComponentByName(dialog, "DepositDate");
        TextMeshProUGUI finishDate = FindTextComponentByName(dialog, "FinishDate");
        TextMeshProUGUI withdrawDate = FindTextComponentByName(dialog, "WithdrawDate");
        TextMeshProUGUI rate = FindTextComponentByName(dialog, "Rate");
        TextMeshProUGUI period = FindTextComponentByName(dialog, "Period");
        TextMeshProUGUI interest = FindTextComponentByName(dialog, "Interest");
        TextMeshProUGUI amount = FindTextComponentByName(dialog, "Amount");

        Assert.IsFalse(finishDate.enabled);
        Assert.IsFalse(period.enabled);
        Assert.AreEqual("Deposit Principal: 1000", principalText.text);
        Assert.AreEqual("Deposit Date: Round 1", depositDate.text);
        Assert.AreEqual("Withdraw Date: Round 1", withdrawDate.text);
        Assert.AreEqual("Interest Rate: " + (data.GetRate() * 100).ToString("F2") + "%", rate.text);
        Assert.AreEqual("Interest: " + data.GetInterest().ToString("F2"), interest.text);
        Assert.AreEqual("Total Amount: " + data.GetTotalAmount().ToString("F2"), amount.text);
    }

    [Test]
    public void ShowDialog_TimeDepositDialog_NoDueNotExpired()
    {
        DepositData data = new DepositData(1000f, 0.0100f, 3, 1);
        data.currentRound = 1;
        data.currentDepositRate = 0.0035f;
        withdrawalManager.ShowDialog(data);
        Assert.AreEqual(1, withdrawalManager.TimePart.childCount);

        GameObject dialog = withdrawalManager.TimePart.GetChild(0).gameObject;
        TextMeshProUGUI warning = FindTextComponentByName(dialog, "Warning");
        TextMeshProUGUI principalText = FindTextComponentByName(dialog, "Principal");
        TextMeshProUGUI depositDate = FindTextComponentByName(dialog, "DepositDate");
        TextMeshProUGUI finishDate = FindTextComponentByName(dialog, "FinishDate");
        TextMeshProUGUI withdrawDate = FindTextComponentByName(dialog, "WithdrawDate");
        TextMeshProUGUI rate = FindTextComponentByName(dialog, "Rate");
        TextMeshProUGUI period = FindTextComponentByName(dialog, "Period");
        TextMeshProUGUI interest = FindTextComponentByName(dialog, "Interest");
        TextMeshProUGUI amount = FindTextComponentByName(dialog, "Amount");

        float newInterest = data.GetPrincipal() * (data.currentDepositRate / 12) * (data.currentRound - data.GetDepositDate());
        float newAmount = newInterest + data.GetPrincipal();
        Assert.AreEqual("Interest Rate: " + (data.GetRate() * 100).ToString("F2") + "% => " + (data.currentDepositRate * 100).ToString("F2") + "%", rate.text);
        Assert.AreEqual("Interest: " + data.GetInterest().ToString("F2") + " => " + newInterest.ToString("F2"), interest.text);
        Assert.AreEqual("Total Amount: " + data.GetTotalAmount().ToString("F2") + " => " + newAmount.ToString("F2"), amount.text);
        Assert.AreEqual("Deposit Principal: " + data.GetPrincipal().ToString(), principalText.text);
        Assert.AreEqual("Deposit Date: Round " + data.GetDepositDate().ToString(), depositDate.text);
        Assert.AreEqual("Finish Date: Round " + data.GetFinishDate().ToString(), finishDate.text);
        Assert.AreEqual("Withdraw Date: Round " + data.currentRound, withdrawDate.text);
        Assert.AreEqual("Deposit Period: " + data.GetPeriod().ToString() + " Rounds", period.text);
        Assert.AreEqual("Warning! If the time deposit is withdrawn in advance, the interest rate is calculated as current deposit.", warning.text);
    }

    [Test]
    public void ShowDialog_TimeDepositDialog_IsDue()
    {
        DepositData data = new DepositData(1000f, 0.0225f, 24, 1);
        withdrawalManager.currentRound = 25;
        data.currentRound = 25;
        data.currentDepositRate = 0.0035f;
        data.CheckExpireDue();
        withdrawalManager.ShowDialog(data);
        Assert.AreEqual(1, withdrawalManager.TimePart.childCount);

        GameObject dialog = withdrawalManager.TimePart.GetChild(0).gameObject;
        TextMeshProUGUI warning = FindTextComponentByName(dialog, "Warning");
        TextMeshProUGUI principalText = FindTextComponentByName(dialog, "Principal");
        TextMeshProUGUI depositDate = FindTextComponentByName(dialog, "DepositDate");
        TextMeshProUGUI finishDate = FindTextComponentByName(dialog, "FinishDate");
        TextMeshProUGUI withdrawDate = FindTextComponentByName(dialog, "WithdrawDate");
        TextMeshProUGUI rate = FindTextComponentByName(dialog, "Rate");
        TextMeshProUGUI period = FindTextComponentByName(dialog, "Period");
        TextMeshProUGUI interest = FindTextComponentByName(dialog, "Interest");
        TextMeshProUGUI amount = FindTextComponentByName(dialog, "Amount");

        float newInterest = data.GetPrincipal() * (data.currentDepositRate / 12) * (data.currentRound - data.GetDepositDate());
        float newAmount = newInterest + data.GetPrincipal();
        Assert.AreEqual("Interest Rate: " + (data.GetRate() * 100).ToString("F2") + "%", rate.text);
        Assert.AreEqual("Interest: " + data.GetInterest().ToString("F2"), interest.text);
        Assert.AreEqual("Total Amount: " + data.GetTotalAmount().ToString("F2"), amount.text);
        Assert.AreEqual("Deposit Principal: " + data.GetPrincipal().ToString(), principalText.text);
        Assert.AreEqual("Deposit Date: Round " + data.GetDepositDate().ToString(), depositDate.text);
        Assert.AreEqual("Finish Date: Round " + data.GetFinishDate().ToString(), finishDate.text);
        Assert.AreEqual("Withdraw Date: Round " + data.currentRound, withdrawDate.text);
        Assert.AreEqual("Deposit Period: " + data.GetPeriod().ToString() + " Rounds", period.text);
        Assert.AreEqual("Warning! Your deposit is due.", warning.text);
    }

    [Test]
    public void ShowDialog_TimeDepositDialog_HasExpired()
    {
        DepositData data = new DepositData(1000f, 0.0135f, 12, 1);
        withdrawalManager.currentRound = 30;
        data.currentRound = 30;
        data.currentDepositRate = 0.0035f;
        data.CheckExpireDue();
        withdrawalManager.ShowDialog(data);
        Assert.AreEqual(1, withdrawalManager.TimePart.childCount);

        GameObject dialog = withdrawalManager.TimePart.GetChild(0).gameObject;
        TextMeshProUGUI warning = FindTextComponentByName(dialog, "Warning");
        TextMeshProUGUI principalText = FindTextComponentByName(dialog, "Principal");
        TextMeshProUGUI depositDate = FindTextComponentByName(dialog, "DepositDate");
        TextMeshProUGUI finishDate = FindTextComponentByName(dialog, "FinishDate");
        TextMeshProUGUI withdrawDate = FindTextComponentByName(dialog, "WithdrawDate");
        TextMeshProUGUI rate = FindTextComponentByName(dialog, "Rate");
        TextMeshProUGUI period = FindTextComponentByName(dialog, "Period");
        TextMeshProUGUI interest = FindTextComponentByName(dialog, "Interest");
        TextMeshProUGUI amount = FindTextComponentByName(dialog, "Amount");

        float newInterest = data.GetPrincipal() * (data.currentDepositRate / 12) * (data.currentRound - data.GetDepositDate());
        float newAmount = newInterest + data.GetPrincipal();
        Assert.AreEqual("Interest Rate: " + (data.GetRate() * 100).ToString("F2") + "%", rate.text);
        Assert.AreEqual("Interest: " + data.GetInterest().ToString("F2"), interest.text);
        Assert.AreEqual("Total Amount: " + data.GetTotalAmount().ToString("F2"), amount.text);
        Assert.AreEqual("Deposit Principal: " + data.GetPrincipal().ToString(), principalText.text);
        Assert.AreEqual("Deposit Date: Round " + data.GetDepositDate().ToString(), depositDate.text);
        Assert.AreEqual("Finish Date: Round " + data.GetFinishDate().ToString(), finishDate.text);
        Assert.AreEqual("Withdraw Date: Round " + data.currentRound, withdrawDate.text);
        Assert.AreEqual("Deposit Period: " + data.GetPeriod().ToString() + " Rounds", period.text);
        Assert.AreEqual("Warning! Your deposit has expired. The deposit interest rate was converted into the current deposit rate.", warning.text);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(playerManager.gameObject);
        Object.DestroyImmediate(withdrawalManager.gameObject);
        Object.DestroyImmediate(timeManager.gameObject);
        Object.DestroyImmediate(currentManager.gameObject);

        Object.DestroyImmediate(noTimeDeposit.gameObject);
        Object.DestroyImmediate(noCurrentDeposit.gameObject);
        Object.DestroyImmediate(TimeItemPrefabOb);
        Object.DestroyImmediate(CurrentItemPrefabOb);
        Object.DestroyImmediate(Timecontent.gameObject);
        Object.DestroyImmediate(Currentcontent.gameObject);
        Object.DestroyImmediate(toTimeButton.gameObject);
        Object.DestroyImmediate(toCurrentButton.gameObject);
        Object.DestroyImmediate(CurrentPart.gameObject);
        Object.DestroyImmediate(TimePart.gameObject);
        Object.DestroyImmediate(DialogOb);
    }

    // Create the component of the TextMeshProUGUI
    private void CreateTextComponent(string text, GameObject uiItem)
    {
        GameObject gameObject = new GameObject(text);
        gameObject.AddComponent<TextMeshProUGUI>();
        gameObject.transform.SetParent(uiItem.transform);
    }

    // Find the component of the content
    // private TextMeshProUGUI FindTextComponentByName(GameObject parent, string name)
    // {
    //     Transform component = parent.transform.Find(name);
    //     if (component != null)
    //     {
    //         return component.GetComponent<TextMeshProUGUI>();
    //     }
    //     return null;
    // }
    private TextMeshProUGUI FindTextComponentByName(GameObject parent, string name)
    {
        if (parent != null)
        {
            foreach (Transform child in parent.transform)
            {
                if (child.name == name)
                {
                    return child.GetComponent<TextMeshProUGUI>();
                }
                var componentInChildren = FindTextComponentByName(child.gameObject, name);
                if (componentInChildren != null)
                    return componentInChildren;
            }
        }
        return null;
    }
}
