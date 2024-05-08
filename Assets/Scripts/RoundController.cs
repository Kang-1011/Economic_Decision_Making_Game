// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundController : MonoBehaviour
{
    PlayerManager playerManager;
    BankRateManager rateManager;

    public Text showRound;

    void Start()
    {
        playerManager = PlayerManager.Instance;
        rateManager = BankRateManager.Instance;
        showRound.text = "Round: " + playerManager.GetRound().ToString();
    }

    // Click the Next button to move to the next round
    public void NextRound()
    {
        playerManager.AddRound();
        UpdateBankRate();
        CurrentDepositManager.Instance.UpdateDeposit();
        DepositManager.Instance.UpdateDeposit();
        FinancialRateManager.Instance.UpdateRates();
        FinancialManager.Instance.UpdateFinancialInterests();
        LoanManager.Instance.AutoRepay();
        
        PlayerManager.Instance.playerCash = PlayerManager.Instance.playerCash + PlayerManager.Instance.preMonthlyRentIncome - PlayerManager.Instance.monthlyPayment;
        PlayerManager.Instance.UpdateHouseValue();
        PlayerManager.Instance.UpdateHouseLoan();
        PlayerManager.Instance.UpdateMonthlyRentIncome();

        showRound.text = "Round: " + playerManager.GetRound().ToString();

        // Check if the current round is 37, then load the "End" scene
        if (playerManager.GetRound() == 37)
        {
            SceneManager.LoadScene("End");
        }
    }

    private void UpdateBankRate()
    {
        // Update the bank rate on 12 round and 24 round
        if (playerManager.GetRound() == 12)
        {
            rateManager.SetDepositRate_Current(0.003f);
            rateManager.SetCreditLoanRate_12Rounds(0.058f);
            rateManager.SetCreditLoanRate_24Rounds(0.062f);
            rateManager.SetCreditLoanRate_36Rounds(0.065f);
        }
        else if (playerManager.GetRound() == 24)
        {
            rateManager.SetDepositRate_Current(0.0025f);
            rateManager.SetDepositRate_3Rounds(0.0125f);
            rateManager.SetDepositRate_6Rounds(0.0145f);
            rateManager.SetDepositRate_12Rounds(0.0165f);
            rateManager.SetDepositRate_24Rounds(0.0215f);
            rateManager.SetDepositRate_36Rounds(0.0245f);
            rateManager.SetCreditLoanRate_12Rounds(0.0535f);
            rateManager.SetCreditLoanRate_24Rounds(0.057f);
            rateManager.SetCreditLoanRate_36Rounds(0.0605f);
        }
    }
}
