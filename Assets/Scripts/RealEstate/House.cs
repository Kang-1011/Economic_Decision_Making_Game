// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House
{
    public int round;
    public int sellRound;
    public string title;
    public float area;
    public float purchasePrice;
    public float marketPrice;
    public float sellPrice;
    public float profitAndLoss;
    public float downPayment;
    public float monthlyPayment;
    public float percentage_change;

    public static List<House> purchasedHouses = new List<House>();
    public static List<House> purchasedHousesHistory = new List<House>();

    public House (int purchaseRound, string houseTitle, float houseArea, float price, float downPay, float monthlyPay)
    {
        round = purchaseRound;
        sellRound = -1; 
        title = houseTitle;
        area = houseArea;
        purchasePrice = price;
        sellPrice = 0.0f;
        profitAndLoss = 0.0f;
        downPayment = downPay;
        monthlyPayment = monthlyPay;
        percentage_change = 0.0f;
        purchasedHouses.Add(this);
        purchasedHousesHistory.Add(this);
    }
}