// All code was written by the team

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RedemptionData
{
    private float principal;
    private float totalAmount;
    private string type;

    public RedemptionData(float principal, string type, float totalAmount)
    {
        this.principal = principal;
        this.type = type;
        this.totalAmount = totalAmount;
    }

    public float GetPrincipal()
    {
        return principal;
    }

    public string GetFinancialType()
    {
        return type;
    }

    public float GetTotalAmount()
    {
        return totalAmount;
    }

}