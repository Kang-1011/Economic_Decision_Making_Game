// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DepositController : MonoBehaviour
{
    public void LoadDepositMain()
    {
        SceneManager.LoadScene("Deposit_Main");
    }

    public void LoadCurrentDeposit()
    {
        SceneManager.LoadScene("Current_Deposit");
    }

    public void LoadTimeDeposit()
    {
        SceneManager.LoadScene("Time_Deposit");
    }
}
