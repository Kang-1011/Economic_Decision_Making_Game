// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AllPurchasedProdoctInfoTests
{
    private AllPurchasedProdoctInfo allPurchasedProdoctInfo;
    [SetUp]
    public void SetUp()
    {
        GameObject gameObject = new GameObject();
        allPurchasedProdoctInfo = gameObject.AddComponent<AllPurchasedProdoctInfo>();
    }

    [Test]
    public void AddAllProduct()
    {
        Assert.AreEqual(0, allPurchasedProdoctInfo.allPurchasedProducts.Count);

        allPurchasedProdoctInfo.AddAllProduct("Product 1", 1000f, "R1", 1);
        Assert.IsNotNull(allPurchasedProdoctInfo);
        Assert.AreEqual(1, allPurchasedProdoctInfo.allPurchasedProducts.Count);
    }

    [Test]
    public void GetAllProduct()
    {
        allPurchasedProdoctInfo.AddAllProduct("Product 1", 1000f, "R1", 1);
        allPurchasedProdoctInfo.AddAllProduct("Product 2", 5000f, "R3", 3);

        Assert.AreEqual(2, allPurchasedProdoctInfo.GetAllProduct().Count);
    }
    
    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(allPurchasedProdoctInfo.gameObject);
    }
}
