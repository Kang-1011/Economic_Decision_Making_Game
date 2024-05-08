// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PurchaseController : MonoBehaviour
{
    public void LoadPurchaseMain()
    {
        SceneManager.LoadScene("Purchase_Main");
    }

    public void LoadInvestment1()
    {
        SceneManager.LoadScene("Investment1");
    }

    public void LoadInvestment2()
    {
        SceneManager.LoadScene("Investment2");
    }

    public void LoadInvestment3()
    {
        SceneManager.LoadScene("Investment3");
    }
}
