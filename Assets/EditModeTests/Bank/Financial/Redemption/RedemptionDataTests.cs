// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RedemptionDataTests
{
    [Test]
    public void RedemptionData_CreateConstructor()
    {
        RedemptionData redemptionData = new RedemptionData(1000f, "R1", 1200f);
        Assert.AreEqual(1000f, redemptionData.GetPrincipal());
        Assert.AreEqual("R1", redemptionData.GetFinancialType());
        Assert.AreEqual(1200f, redemptionData.GetTotalAmount());
    }
}
