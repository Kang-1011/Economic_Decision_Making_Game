// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanManager : MonoBehaviour
{
    public static LoanManager Instance { get; private set; }

    private List<LoanData> loanDatas = new List<LoanData>();
    private int defaultTimes = 0;

    void Awake()
    {
        // Initialize the singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public List<LoanData> GetList()
    {
        return loanDatas;
    }

    // Add a new loan
    public void AddLoan(int loanAmount, float rate, int period, int loanDate)
    {
        loanDatas.Add(new LoanData(loanAmount, rate, period, loanDate));
    }

    public void RemoveLoan(int index)
    {
        loanDatas.RemoveAt(index);
    }

    public void FinishRepay(int index)
    {
        loanDatas[index].FinishRepay();
    }

    public void AddRepayedRound(int index)
    {
        loanDatas[index].AddRepayedRound();
    }

    public void ClearDefault(int index)
    {
        loanDatas[index].RepayDefault();
    }

    public void AutoRepay()
    {
        List<int> moveLoans = new List<int>();
        for (int i = 0; i < loanDatas.Count; i++)
        {
            loanDatas[i].AddRepayedRound();
            if (loanDatas[i].GetFinishDate() > PlayerManager.Instance.GetRound())
            {
                if (loanDatas[i].GetMonthlyRepayAmount() <= PlayerManager.Instance.GetPlayerCash())
                {
                    loanDatas[i].UpdateRemainAmount();
                    PlayerManager.Instance.SubtractPlayerCash(loanDatas[i].GetMonthlyRepayAmount());
                    CreditRateManager.Instance.OntimeRepay();
                    loanDatas[i].FinishRepay();
                }

                if (loanDatas[i].GetRepayedRound() > 1)
                {
                    if (!loanDatas[i].GetIsLastRoundRepayed())
                    {
                        loanDatas[i].AddDefaultRound();
                        defaultTimes++;
                        CreditRateManager.Instance.AddDefaultTime();
                    }
                }

                if (loanDatas[i].GetDefaultAmount() > 0 || loanDatas[i].GetDefaultRound() > 0)
                {
                    loanDatas[i].UpdateDefault();
                }
            }
            else if (loanDatas[i].GetFinishDate() == PlayerManager.Instance.GetRound())
            {
                if (loanDatas[i].GetMonthlyRepayAmount() <= PlayerManager.Instance.GetPlayerCash())
                {
                    loanDatas[i].UpdateRemainAmount();
                    PlayerManager.Instance.SubtractPlayerCash(loanDatas[i].GetMonthlyRepayAmount());
                    CreditRateManager.Instance.OntimeRepay();
                    loanDatas[i].FinishRepay();
                }

                if (!loanDatas[i].GetIsLastRoundRepayed())
                {
                    loanDatas[i].AddDefaultRound();
                    defaultTimes++;
                    CreditRateManager.Instance.AddDefaultTime();
                }

                if (loanDatas[i].GetDefaultAmount() > 0 || loanDatas[i].GetDefaultRound() > 0)
                {
                    loanDatas[i].UpdateDefault();
                }

                if (loanDatas[i].GetDefaultAmount() == 0 && loanDatas[i].GetDefaultRound() == 0 && loanDatas[i].GetIsLastRoundRepayed())
                {
                    moveLoans.Add(i);
                }
            }
            else
            {
                if(loanDatas[i].GetFinishDate() == PlayerManager.Instance.GetRound() - 1)
                {
                    if (!loanDatas[i].GetIsLastRoundRepayed())
                    {
                        loanDatas[i].AddDefaultRound();
                        defaultTimes++;
                        CreditRateManager.Instance.AddDefaultTime();
                    }
                }

                if (loanDatas[i].GetDefaultAmount() > 0 || loanDatas[i].GetDefaultRound() > 0)
                {
                    loanDatas[i].UpdateDefault();
                }

                if (loanDatas[i].GetDefaultAmount() == 0 && loanDatas[i].GetDefaultRound() == 0 && loanDatas[i].GetIsLastRoundRepayed())
                {
                    moveLoans.Add(i);
                }
            }
        }
        if (moveLoans.Count > 0)
        {
            int haveRemovedLoans = 0;
            foreach (int moveLoan in moveLoans)
            {
                loanDatas.RemoveAt(moveLoan - haveRemovedLoans);
                haveRemovedLoans += 1;
            }
        }
        PlayerManager.Instance.SetBankLoanValue();
    }

    public float CalculateTotalLoan()
    {
        float totalLoan = 0;
        for(int i = 0; i < loanDatas.Count; i++)
        {
            totalLoan += loanDatas[i].GetRemainAmount();
        }
        return totalLoan;
    }

    public int CalculateTotalLoanAmount()
    {
        int totalLoanAmount = 0;
        for(int i = 0; i < loanDatas.Count; i++)
        {
            totalLoanAmount += loanDatas[i].GetLoanAmount();
        }
        return totalLoanAmount;
    }

    public int GetDefaultTimes()
    {
        return defaultTimes;
    }
}
