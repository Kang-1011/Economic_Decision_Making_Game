// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WithdrawalManager : MonoBehaviour
{
    public TextMeshProUGUI noTimeDeposit;
    public TextMeshProUGUI noCurrentDeposit;
    public GameObject TimeItemPrefab;
    public GameObject CurrentItemPrefab;
    public Transform Timecontent;
    public Transform Currentcontent;
    public Button toTimeButton;
    public Button toCurrentButton;
    public Transform CurrentPart;
    public Transform TimePart;

    public GameObject Dialog;
    public GameObject Toast;

    public float currentDepositRate;
    public int currentRound;
    public List<DepositData> Deposits;
    public List<CurrentDepositData> CurrentDeposits;
    public List<GameObject> timeDepositItems = new List<GameObject>();
    public List<GameObject> currentDepositItems = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        currentDepositRate = BankRateManager.Instance.GetDepositRate_Current();
        currentRound = PlayerManager.Instance.GetRound();
        Deposits = DepositManager.Instance.depositDatas;
        CurrentDeposits = CurrentDepositManager.Instance.depositDatas;

        ShowCurrentDepositItem();
        toTimeButton.onClick.AddListener(ShowTimeDepositItem);
        toCurrentButton.onClick.AddListener(ShowCurrentDepositItem);

        noTimeDeposit.gameObject.SetActive(false);
    }

    // Show time deposit items after checking
    public void ShowTimeDepositItem()
    {
        noCurrentDeposit.gameObject.SetActive(false);
        if (Deposits.Count > 0)
        {
            noTimeDeposit.gameObject.SetActive(false);

            // Make UI elements enough to display all time deposit items
            while (timeDepositItems.Count < Deposits.Count)
            {
                GameObject newItem = Instantiate(TimeItemPrefab, Timecontent);
                timeDepositItems.Add(newItem);
            }

            // Loop reading data from the time deposit list to showm
            for (int i = 0; i < timeDepositItems.Count; i++)
            {
                GameObject uiItem = timeDepositItems[i];
                if (i < Deposits.Count)
                {
                    DepositData data = Deposits[i];
                    uiItem.SetActive(true);
                    UpdateTimeDepositItemUI(uiItem, data);
                }
                else
                {
                    uiItem.SetActive(false);
                }
            }
        }
        else
        {
            noTimeDeposit.gameObject.SetActive(true);
        }
    }

    // Update time deposit items UI
    public void UpdateTimeDepositItemUI(GameObject uiItem, DepositData data)
    {
        TextMeshProUGUI principal = FindTextComponentByName(uiItem, "Principal");
        TextMeshProUGUI rate = FindTextComponentByName(uiItem, "Rate");
        TextMeshProUGUI period = FindTextComponentByName(uiItem, "Period");
        TextMeshProUGUI depositDate = FindTextComponentByName(uiItem, "DepositDate");
        TextMeshProUGUI finishDate = FindTextComponentByName(uiItem, "FinishDate");
        Button withdrawButton = FindButtonComponentByName(uiItem, "Button");

        principal.text = data.GetPrincipal().ToString();
        rate.text = (data.GetRate() * 100).ToString("F2") + "%";
        period.text = data.GetPeriod().ToString() + "\nRounds";
        depositDate.text = "Round\n" + data.GetDepositDate().ToString();
        finishDate.text = "Round\n" + data.GetFinishDate().ToString();

        if (withdrawButton != null)
        {
            data.currentDepositRate = currentDepositRate;
            data.currentRound = currentRound;
            withdrawButton.onClick.RemoveAllListeners();
            withdrawButton.onClick.AddListener(() => ShowDialog(data));
        }
    }

    // Show current deposit items
    public void ShowCurrentDepositItem()
    {
        noTimeDeposit.gameObject.SetActive(false);
        if (CurrentDeposits.Count > 0)
        {
            noCurrentDeposit.gameObject.SetActive(false);

            // Make UI elements enough to display all current deposit items
            while (currentDepositItems.Count < CurrentDeposits.Count)
            {
                GameObject newItem = Instantiate(CurrentItemPrefab, Currentcontent);
                currentDepositItems.Add(newItem);
            }

            // Loop reading data from the current deposit list to show
            for (int i = 0; i < currentDepositItems.Count; i++)
            {
                GameObject uiItem = currentDepositItems[i];
                if (i < CurrentDeposits.Count)
                {
                    CurrentDepositData data = CurrentDeposits[i];
                    uiItem.SetActive(true);
                    UpdateCurrentDepositItemUI(uiItem, data);
                }
                else
                {
                    uiItem.SetActive(false);
                }
            }
        }
        else
        {
            noCurrentDeposit.gameObject.SetActive(true);
        }
    }

    // Update current deposit items UI
    public void UpdateCurrentDepositItemUI(GameObject uiItem, CurrentDepositData data)
    {
        TextMeshProUGUI principal = FindTextComponentByName(uiItem, "Principal");
        TextMeshProUGUI rate = FindTextComponentByName(uiItem, "Rate");
        TextMeshProUGUI depositDate = FindTextComponentByName(uiItem, "DepositDate");
        Button withdrawButton = FindButtonComponentByName(uiItem, "Button");

        principal.text = data.GetPrincipal().ToString();
        rate.text = (data.GetRate() * 100).ToString() + "%";
        depositDate.text = "Round " + data.GetDepositDate().ToString();
        if (withdrawButton != null)
        {
            withdrawButton.onClick.RemoveAllListeners();
            withdrawButton.onClick.AddListener(() => ShowDialog(data));
        }
    }

    // Find the component of the content
    TextMeshProUGUI FindTextComponentByName(GameObject parent, string name)
    {
        TextMeshProUGUI[] allTextComponents = parent.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textComp in allTextComponents)
        {
            if (textComp.gameObject.name == name)
            {
                return textComp;
            }
        }
        return null;
    }

    // Find the component of the button in the GameObject
    Button FindButtonComponentByName(GameObject parent, string name)
    {
        Button button = parent.transform.Find(name)?.GetComponent<Button>();
        // If the button can be found in 'parent', return it
        if (button != null)
        {
            return button;
        }
        else
        {
            //Recursively search through its children to find the button
            foreach (Transform child in parent.transform)
            {
                button = FindButtonComponentByName(child.gameObject, name);
                if (button != null)
                {
                    return button;
                }
            }
            return null;
        }
    }

    // Show the currentdeposit dialog after clicked the withdraw button
    public void ShowDialog(CurrentDepositData data)
    {
        GameObject dialog = Instantiate(Dialog, CurrentPart);

        TextMeshProUGUI principal = FindTextComponentByName(dialog, "Principal");
        TextMeshProUGUI depositDate = FindTextComponentByName(dialog, "DepositDate");
        TextMeshProUGUI finishDate = FindTextComponentByName(dialog, "FinishDate");
        TextMeshProUGUI withdrawDate = FindTextComponentByName(dialog, "WithdrawDate");
        TextMeshProUGUI rate = FindTextComponentByName(dialog, "Rate");
        TextMeshProUGUI period = FindTextComponentByName(dialog, "Period");
        TextMeshProUGUI interest = FindTextComponentByName(dialog, "Interest");
        TextMeshProUGUI amount = FindTextComponentByName(dialog, "Amount");
        Button confirmButton = FindButtonComponentByName(dialog, "ConfirmButton");

        // Hide these two messages
        finishDate.enabled = false;
        period.enabled = false;

        principal.text = "Deposit Principal: " + data.GetPrincipal().ToString();
        depositDate.text = "Deposit Date: Round " + data.GetDepositDate().ToString();
        withdrawDate.text = "Withdraw Date: Round " + currentRound;
        rate.text = "Interest Rate: " + (data.GetRate() * 100).ToString("F2") + "%";
        interest.text = "Interest: " + data.GetInterest().ToString("F2");
        amount.text = "Total Amount: " + data.GetTotalAmount().ToString("F2");

        // Add two functions to the confirmButton
        confirmButton.onClick.AddListener(() =>
        {
            WithdrawSubmit(data);
            Toast.SetActive(true);
        });
        Dialog.SetActive(true);
    }

    // Show the timedeposit dialog through method overloading
    public void ShowDialog(DepositData data)
    {
        GameObject dialog = Instantiate(Dialog, TimePart);

        TextMeshProUGUI warning = FindTextComponentByName(dialog, "Warning");
        TextMeshProUGUI principal = FindTextComponentByName(dialog, "Principal");
        TextMeshProUGUI depositDate = FindTextComponentByName(dialog, "DepositDate");
        TextMeshProUGUI finishDate = FindTextComponentByName(dialog, "FinishDate");
        TextMeshProUGUI withdrawDate = FindTextComponentByName(dialog, "WithdrawDate");
        TextMeshProUGUI rate = FindTextComponentByName(dialog, "Rate");
        TextMeshProUGUI period = FindTextComponentByName(dialog, "Period");
        TextMeshProUGUI interest = FindTextComponentByName(dialog, "Interest");
        TextMeshProUGUI amount = FindTextComponentByName(dialog, "Amount");
        Button confirmButton = FindButtonComponentByName(dialog, "ConfirmButton");

        rate.text = "Interest Rate: " + (data.GetRate() * 100).ToString("F2") + "%";
        interest.text = "Interest: " + data.GetInterest().ToString("F2");
        amount.text = "Total Amount: " + data.GetTotalAmount().ToString("F2");
        principal.text = "Deposit Principal: " + data.GetPrincipal().ToString();
        depositDate.text = "Deposit Date: Round " + data.GetDepositDate().ToString();
        finishDate.text = "Finish Date: Round " + data.GetFinishDate().ToString();
        withdrawDate.text = "Withdraw Date: Round " + currentRound;
        period.text = "Deposit Period: " + data.GetPeriod().ToString() + " Rounds";
        // Check whether the time deposit is due and has expired to show the different massage
        if (data.GetDue() == false && data.GetExpire() == false)
        {
            warning.text = "Warning! If the time deposit is withdrawn in advance, the interest rate is calculated as current deposit.";
            float newInterest = data.GetPrincipal() * (currentDepositRate / 12) * (currentRound - data.GetDepositDate());
            float newAmount = newInterest + data.GetPrincipal();

            rate.text = "Interest Rate: " + (data.GetRate() * 100).ToString("F2") + "% => " + (currentDepositRate * 100).ToString("F2") + "%";
            interest.text = "Interest: " + data.GetInterest().ToString("F2") + " => " + newInterest.ToString("F2");
            amount.text = "Total Amount: " + data.GetTotalAmount().ToString("F2") + " => " + newAmount.ToString("F2");
        }
        else if (data.GetDue() == true && data.GetExpire() == false)
        {
            warning.text = "Warning! Your deposit is due.";
        }
        else if (data.GetDue() == false && data.GetExpire() == true)
        {
            warning.text = "Warning! Your deposit has expired. The deposit interest rate was converted into the current deposit rate.";
        }

        // Add two functions to the confirmButton
        confirmButton.onClick.AddListener(() =>
        {
            WithdrawSubmit(data);
            Toast.SetActive(true);
        });
        Dialog.SetActive(true);
    }

    // Change the related amount
    private void WithdrawSubmit(CurrentDepositData data)
    {
        CurrentDepositManager.Instance.AddWithdrawalCurrentDeposit(data);
        CurrentDepositManager.Instance.RemoveDeposit(data);
        PlayerManager.Instance.AddPlayerCash(data.GetTotalAmount());
        PlayerManager.Instance.SetBankDepositValue();
    }

    // Change the related amount through method overloading
    private void WithdrawSubmit(DepositData data)
    {
        if (data.GetDue() == false && data.GetExpire() == false)
        {
            data.EarlyWithdraw();
        }
        DepositManager.Instance.AddWithdrawalDeposit(data);
        DepositManager.Instance.RemoveDeposit(data);
        PlayerManager.Instance.AddPlayerCash(data.GetTotalAmount());
        PlayerManager.Instance.SetBankDepositValue();
    }
}