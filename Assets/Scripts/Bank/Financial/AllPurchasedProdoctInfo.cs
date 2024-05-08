// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AllPurchasedProdoctInfo : MonoBehaviour
{
    public List<FinancialData> allPurchasedProducts = new List<FinancialData>();

    public void AddAllProduct(string name, float principal, string type, int purchaseDate)
    {
        allPurchasedProducts.Add(new FinancialData(name, principal, type, purchaseDate));
    }

    public List<FinancialData> GetAllProduct()
    {
        return allPurchasedProducts;
    }
}
