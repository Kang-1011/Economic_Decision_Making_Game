// All code was written by the team

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepaymentManager : MonoBehaviour
{
    public GameObject repaymentItemPrefab;
    public GameObject noCreditLoans;
    public Transform content;
    public GameObject toast;
    public TextMeshProUGUI toastText;

    void Start()
    {
        ShowAllLoans();
    }

    public void ShowAllLoans()
    {
        List<LoanData> loanDatas = LoanManager.Instance.GetList();

        if(loanDatas.Count == 0)
        {
            noCreditLoans.SetActive(true);
            return;
        }
        else
        {
            noCreditLoans.SetActive(false);
        }

        float amount;
        int i = 0;
        foreach(LoanData data in loanDatas)
        {
            GameObject newItem = Instantiate(repaymentItemPrefab, content);
            TextMeshProUGUI loanAmount = FindTextComponentByName(newItem, "LoanAmount");
            TextMeshProUGUI rate = FindTextComponentByName(newItem, "Rate");
            TextMeshProUGUI repayAmount = FindTextComponentByName(newItem, "RepayAmount");
            TextMeshProUGUI period = FindTextComponentByName(newItem, "Period");
            TextMeshProUGUI remainAmount = FindTextComponentByName(newItem, "RemainAmount");
            TextMeshProUGUI monthlyRepayAmount = FindTextComponentByName(newItem, "Monthly");
            TextMeshProUGUI loanDate = FindTextComponentByName(newItem, "LoanDate");
            TextMeshProUGUI finishDate = FindTextComponentByName(newItem, "FinishDate");
            TextMeshProUGUI warning = FindTextComponentByName(newItem, "Warning");

            Button fullButton = FindButtonComponentByName(newItem, "FullButton");
            Button repayButton = FindButtonComponentByName(newItem, "RepayButton");
            int index = i;
            fullButton.onClick.AddListener(() => FullButtonClicked(index));

            if (data.GetRepayedRound() > 0)
            {
                if (data.GetIsRepayed() && data.GetDefaultAmount() == 0 && data.GetDefaultRound() == 0)
                {
                    repayButton.interactable = false;
                    warning.color = Color.black;
                    warning.text = "You don't have a delinquent loan";
                }
                else
                {
                    repayButton.interactable = true;
                    repayButton.onClick.AddListener(() => RepayButtonClicked(index));
                    amount = data.GetDefaultRound() * data.GetMonthlyRepayAmount() + data.GetDefaultAmount();
                    warning.color = Color.red;
                    warning.text = "You need to repay " + amount.ToString("F2") + " yuan.";
                }
            }
            else
            {
                repayButton.interactable = false;
            }

            loanAmount.text = "Loan Amount: " + data.GetLoanAmount();
            rate.text = "Rate: " + data.GetRate() * 100 + "%";
            repayAmount.text = "Repay Amount: " + data.GetPeriod();
            period.text = "Period: " + data.GetPeriod();
            remainAmount.text = "Remain Amount: " + data.GetRemainAmount().ToString("F2");
            monthlyRepayAmount.text = "Monthly Repay Amount: " + data.GetMonthlyRepayAmount().ToString("F2");
            loanDate.text = "Loan Date: " + data.GetLoanDate();
            finishDate.text = "Finish Date: " + data.GetFinishDate();
        }
    }

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

    Button FindButtonComponentByName(GameObject parent, string name)
    {
        return parent.transform.Find(name).GetComponent<Button>();
    }

    public void FullButtonClicked(int index)
    {
        List<LoanData> loanDatas = LoanManager.Instance.GetList();
        LoanData loanData = loanDatas[index];
        if (loanData.GetRemainAmount() <= PlayerManager.Instance.GetPlayerCash())
        {
            PlayerManager.Instance.SubtractPlayerCash(loanData.GetRemainAmount());
            LoanManager.Instance.RemoveLoan(index);
            PlayerManager.Instance.SetBankLoanValue();
            toast.SetActive(true);
            toastText.text = "Your repayment is successful!";
        }
        else
        {
            toast.SetActive(true);
            toastText.text = "You don't have enough money!";
        }
    }

    public void RepayButtonClicked(int index)
    {
        List<LoanData> loanDatas = LoanManager.Instance.GetList();
        LoanData loanData = loanDatas[index];
        float repayAmount = loanData.GetDefaultRound() * loanData.GetMonthlyRepayAmount() + loanData.GetDefaultAmount();
        if (loanData.GetIsRepayed() && loanData.GetRepayedRound() <= loanData.GetPeriod())
        {
            repayAmount += loanData.GetMonthlyRepayAmount();
        }

        if (repayAmount <= PlayerManager.Instance.GetPlayerCash())
        {
            PlayerManager.Instance.SubtractPlayerCash(repayAmount);
            LoanManager.Instance.FinishRepay(index);
            LoanManager.Instance.ClearDefault(index);
            if (loanData.GetFinishDate() <= PlayerManager.Instance.GetRound())
            {
                LoanManager.Instance.RemoveLoan(index);
            }
            PlayerManager.Instance.SetBankLoanValue();
            toast.SetActive(true);
            toastText.text = "Your repayment is successful!";
        }
        else
        {
            toast.SetActive(true);
            toastText.text = "You don't have enough money!";
        }
    }
}
