// All code was written by the team

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class HousePriceManager : MonoBehaviour
{
    public int currentRound;

    public Text houseAText;
    public Text houseBText;
    public Text houseCText;
    public Text houseDText;

    public List<List<int>> prices; // List to store prices for each round and house

    public void Start()
    {
        currentRound = PlayerManager.Instance.GetRound();
        // currentRound = 1;
        
        // Load prices from CSV
        LoadPrices("prices");

        // Update house prices for the current round
        UpdateHousePrices();
    }

    public void LoadPrices(string fileName)
    {
        // Load CSV file from Resources folder
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        
        if (csvFile != null)
        {
            // Split lines and then split each line into values
            List<string> lines = csvFile.text.Split('\n').ToList();
            prices = new List<List<int>>();

            foreach (string line in lines)
            {
                // Trim and remove empty lines
                string trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine))
                {
                    // Split the trimmed line into values
                    List<int> roundPrices = trimmedLine.Split(' ').Select(int.Parse).ToList();
                    prices.Add(roundPrices);
                }
            }
        }
        else
        {
            Debug.LogError("CSV file not found!");
        }
    }

    public void UpdateHousePrices()
    {
        if (currentRound >= 1 && currentRound <= prices.Count)
        {
            List<int> currentRoundPrices = prices[currentRound - 1];

            // Update the UI text components with the current prices
            if (houseAText != null) // Check if houseAText is assigned before accessing it
            {
                houseAText.text = currentRoundPrices[0].ToString() + "/m2";
            }

            if (houseBText != null) // Check if houseBText is assigned before accessing it
            {
                houseBText.text = currentRoundPrices[1].ToString() + "/m2";
            }
            
            if (houseCText != null) // Check if houseCText is assigned before accessing it
            {
                houseCText.text = currentRoundPrices[2].ToString() + "/m2";
            }

            if (houseDText != null) // Check if houseDText is assigned before accessing it
            {
                houseDText.text = currentRoundPrices[3].ToString() + "/m2";
            }
        }
        else
        {
            Debug.LogError("Invalid Round Index!");
        }
    }
}
