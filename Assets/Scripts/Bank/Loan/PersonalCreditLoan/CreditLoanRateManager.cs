// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreditLoanRateManager : MonoBehaviour
{
    BankRateManager rateManager;

    public TextMeshProUGUI cRate_12Rounds;
    public TextMeshProUGUI cRate_24Rounds;
    public TextMeshProUGUI cRate_36Rounds;

    public TextMeshProUGUI creditRating;
    public TextMeshProUGUI creditScore;

    void Awake()
    {
        rateManager = BankRateManager.Instance;
        UpdateLoanRate();
    }

    public void UpdateLoanRate()
    {
        cRate_12Rounds.text = "12 Rounds: " + (rateManager.GetCreditLoanRate_12Rounds() * 100).ToString() + "%";
        cRate_24Rounds.text = "24 Rounds: " + (rateManager.GetCreditLoanRate_24Rounds() * 100).ToString() + "%";
        cRate_36Rounds.text = "36 Rounds: " + (rateManager.GetCreditLoanRate_36Rounds() * 100).ToString() + "%";

        CreditRateManager.Instance.UpdateCreditRating();

        creditRating.text = "Credit Rating: " + CreditRateManager.Instance.GetCreditRating();
        creditScore.text = "Your Credit Score: " + CreditRateManager.Instance.GetCreditScore();
    }
}
