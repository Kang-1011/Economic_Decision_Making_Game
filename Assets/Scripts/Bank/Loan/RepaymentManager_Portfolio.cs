// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepaymentManager_Portfolio : MonoBehaviour
{
    public GameObject repaymentItemPrefab;
    public GameObject noCreditLoans;
    public Transform content;

    void Start()
    {
        ShowAllLoans();
    }

    public void ShowAllLoans()
    {
        List<LoanData> loanDatas = LoanManager.Instance.GetList();

        if (loanDatas.Count == 0)
        {
            noCreditLoans.SetActive(true);
            return;
        }
        else
        {
            noCreditLoans.SetActive(false);
        }

        foreach (LoanData data in loanDatas)
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

            fullButton.gameObject.SetActive(false);
            repayButton.gameObject.SetActive(false);

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
}
