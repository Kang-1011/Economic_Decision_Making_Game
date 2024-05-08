// All code was written by the team

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnManager : MonoBehaviour
{
    public Text houseText;
    public Button purchaseBtn;
    public static Button[] purchaseBtns = new Button[4];
    public static List<bool> states = new List<bool> {true, true, true, true};

    public void Start()
    {
        purchaseBtns = new Button[4];
        switch(houseText.text)
        {
            case "House A":
                purchaseBtns[0] = purchaseBtn;
                break;
            case "House B":
                purchaseBtns[1] = purchaseBtn;
                break;
            case "House C":
                purchaseBtns[2] = purchaseBtn;
                break;
            case "House D":
                purchaseBtns[3] = purchaseBtn;
                break;
            default:
                Debug.LogError("Invalid House Identifier!");
                break;
        }
        purchaseBtn.interactable = states[Array.IndexOf(purchaseBtns, purchaseBtn)];
    }
}
