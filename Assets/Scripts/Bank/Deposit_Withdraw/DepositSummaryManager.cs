// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DepositSummaryManager : MonoBehaviour
{
    public TextMeshProUGUI SummaryText;
    public Text totalPrincipalText;
    public Text totalIncomeText;
    public List<DepositData> timeDepositItem;
    public List<DepositData> timeWithdrawalItem;
    public List<CurrentDepositData> currentDepositItem;
    public List<CurrentDepositData> currentWithdrawalItem;

    public float totalPrincipal;
    public float totalRevenue;

    void Start()
    {
        timeDepositItem = DepositManager.Instance.depositDatas;
        timeWithdrawalItem = DepositManager.Instance.withdrawDatas;
        currentDepositItem = CurrentDepositManager.Instance.depositDatas;
        currentWithdrawalItem = CurrentDepositManager.Instance.withdrawDatas;
        totalPrincipal = DepositManager.Instance.CalculateTotalPrincipal() + CurrentDepositManager.Instance.CalculateTotalPrincipal();
        totalRevenue = DepositManager.Instance.CalculateTotalRevenue() + CurrentDepositManager.Instance.CalculateTotalRevenue();
        DisplayDepositSummary();
    }

    public void DisplayDepositSummary()
    {
        totalPrincipalText.text = "￥ " + totalPrincipal.ToString("F2");
        totalIncomeText.text = "￥ " + totalRevenue.ToString("F2");

        if (timeWithdrawalItem.Count == 0 && currentWithdrawalItem.Count == 0 && 
            timeDepositItem.Count == 0 && currentDepositItem.Count == 0)
        {
            SummaryText.text = "You have not deposited any cash in the deposit module.\n";
        } else if ((timeWithdrawalItem.Count != 0 || timeDepositItem.Count != 0) &&
                    (currentWithdrawalItem.Count == 0 && currentDepositItem.Count == 0))
        {
            SummaryText.text = "You have chosen to make time deposits.\n";
        } else if ((currentWithdrawalItem.Count != 0 || currentDepositItem.Count != 0) &&
                    (timeWithdrawalItem.Count == 0 && timeDepositItem.Count == 0))
        {
            SummaryText.text = "You have chosen to make current deposits.\n";
        } else
        {
            SummaryText.text = "You have chosen to make current deposits and time deposits.\n";
        }
        SummaryText.text = SummaryText.text + "\nCurrent deposits and time deposits, as principal-guranteed finanicial products, " + 
        "carry very low risk and offer stable income.Compared to current deposits, time deposits provide higher interest rates, " + 
        "but the flexibility of the funds is lower. Both are suitable for investors with a low risk tolerance.";
    }
}
