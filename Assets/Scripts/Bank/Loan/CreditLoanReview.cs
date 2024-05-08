// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditLoanReview : MonoBehaviour
{
    public TextMeshProUGUI totalDefaultTimes;
    public TextMeshProUGUI summary;

    // Start is called before the first frame update
    void Start()
    {
        ShowSummary();
    }

    private void ShowSummary()
    {
        totalDefaultTimes.text = "Total Default Times: " + LoanManager.Instance.GetDefaultTimes();

        if (LoanManager.Instance.GetDefaultTimes() < 1)
        {
            summary.text = "Summary: " + "You have not defaulted on personal credit loans, please maintain the good " +
                "habit of making timely repayments in reality as well.";
        }
        else
        {
            summary.text = "Summary: " + "You have defaulted on a personal credit loan. Defaulting negatively affects your " +
                "credit evaluation, impacting your ability to secure credit loans in the future. Additionally, defaulting " +
                "results in significant penalties, causing financial loss. Therefore, please cultivate the good habit of " +
                "making timely repayments in reality to avoid defaulting.";
        }
    }
}
