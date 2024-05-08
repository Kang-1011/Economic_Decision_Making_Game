// All code was written by the team

using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    public void LoadStart()
    {
        SceneManager.LoadScene("Start");
        PlayerManager.Instance.InitializePlayerAssets();
    }

    public void LoadPortfolio()
    {
        SceneManager.LoadScene("Portfolio");
    }

    public void LoadNews()
    {
        SceneManager.LoadScene("News");
    }

    public void LoadRealEstateMain()
    {
        SceneManager.LoadScene("RealEstate_Main");
    }

    public void LoadBankMain()
    {
        SceneManager.LoadScene("Bank_Main");
    }

    public void LoadSecuritiesMain()
    {
        SceneManager.LoadScene("Futures_Stock");
    }

    public void LoadHome()
    {
        SceneManager.LoadScene("Home");
    }
}
