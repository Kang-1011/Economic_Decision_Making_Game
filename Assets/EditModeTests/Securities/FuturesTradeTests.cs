// All code was written by the team

using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class FuturesTradeTests
{
    FuturesTrade futuresTradeInstance = null;

    [SetUp]
    public void Setup()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Futures_Stock/Futures_Stock.unity");
        GameObject[] rootObjects = EditorSceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            if (rootObject.name == "FuturesMarketWithCanvas")
            {
                GameObject baseGameObject = rootObject.transform.Find("FuturesTrade").gameObject;
                futuresTradeInstance = baseGameObject.AddComponent<FuturesTrade>();
                futuresTradeInstance.PriceInputField = baseGameObject.transform.Find("PriceInput").GetComponent<InputField>();
                futuresTradeInstance.QuantityInputField = baseGameObject.transform.Find("QuantityInput").GetComponent<InputField>();
                futuresTradeInstance.TotalValueInputField = baseGameObject.transform.Find("ValueInput").GetComponent<InputField>();
            }
        }
    }

    [Test]
    public void UpdateTotal_EnterQuantity_AutoUpdate()
    {
        // Arrange
        futuresTradeInstance.PriceInputField.text = "25.00";
        futuresTradeInstance.QuantityInputField.text = "4";

        // Act
        futuresTradeInstance.UpdateTotal(futuresTradeInstance.QuantityInputField.text);

        // Assert
        Assert.AreEqual("100.00", futuresTradeInstance.TotalValueInputField.text);
    }

    [Test]
    public void ValidateInput_FloatQuantity_ReturnInteger()
    {
        // Arrange
        futuresTradeInstance.PriceInputField.text = "25.00";
        futuresTradeInstance.QuantityInputField.text = "4.20";

        // Act
        futuresTradeInstance.ValidateInput(futuresTradeInstance.QuantityInputField.text);

        // Assert
        Assert.AreEqual("420", futuresTradeInstance.QuantityInputField.text);
    }
}
