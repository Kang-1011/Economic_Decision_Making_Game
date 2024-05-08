// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoanController : MonoBehaviour
{
    public void LoadRepay()
    {
        SceneManager.LoadScene("Repay_Main");
    }

    public void LoadMakeLoan()
    {
        SceneManager.LoadScene("Personal_Loan");
    }

    public void LoadLoanKnowledge()
    {
        SceneManager.LoadScene("Loan");
    }

    public void LoadChooseProperty()
    {
        SceneManager.LoadScene("Choose_a_Property");
    }

    public void LoadHouseLoan1()
    {
        SceneManager.LoadScene("House_Loan");
    }

    public void LoadPersonalLoan()
    {
        SceneManager.LoadScene("Personal_Loan");
    }
}
