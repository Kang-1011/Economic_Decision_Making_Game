// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BankController : MonoBehaviour
{
    public void LoadHome()
    {
        SceneManager.LoadScene("Home");
    }
    
    public void LoadBankMain()
    {
        SceneManager.LoadScene("Bank_Main");
    }

    public void LoadDepositWithdrawMain()
    {
        SceneManager.LoadScene("Deposit_Withdraw_Main");
    }

    public void LoadFinancialMain()
    {
        SceneManager.LoadScene("Financial_Main");
    }

    public void LoadFinancial()
    {
        SceneManager.LoadScene("Financial");
    }

    public void LoadLoanMain()
    {
        SceneManager.LoadScene("Loan_Main");
    }
}
