// All code was written by the team

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class HouseRentManager : MonoBehaviour
{
    public int currentRound;

    public Text houseAText;
    public Text houseBText;
    public Text houseCText;
    public Text houseDText;

    public List<List<int>> rents; // List to store rents for each round and house

    public void Start()
    {
        currentRound = PlayerManager.Instance.GetRound();
        // currentRound = 1;

        // Load rents from CSV
        LoadRents("rents");

        // Update house rents for the current round
        UpdateHouseRents();
    }

    public void LoadRents(string fileName)
    {
        // Load CSV file from Resources folder
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);

        if (csvFile != null)
        {
            // Split lines and then split each line into values
            List<string> lines = csvFile.text.Split('\n').ToList();
            rents = new List<List<int>>();

            foreach (string line in lines)
            {
                // Trim and remove empty lines
                string trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine))
                {
                    // Split the trimmed line into values
                    List<int> roundRents = trimmedLine.Split(' ').Select(int.Parse).ToList();
                    rents.Add(roundRents);
                }
            }
        }
        else
        {
            Debug.LogError("CSV file not found!");
        }
    }

    public void UpdateHouseRents()
    {
        if (currentRound >= 1 && currentRound <= rents.Count)
        {
            List<int> currentRoundRents = rents[currentRound - 1];

            // Update the UI text components with the current rents
            if (houseAText != null) // Check if houseAText is assigned before accessing it
            {
                houseAText.text = currentRoundRents[0].ToString() + "/Month";
            }

            if (houseBText != null) // Check if houseBText is assigned before accessing it
            {
                houseBText.text = currentRoundRents[1].ToString() + "/Month";
            }

            if (houseCText != null) // Check if houseCText is assigned before accessing it
            {
                houseCText.text = currentRoundRents[2].ToString() + "/Month";
            }

            if (houseDText != null) // Check if houseDText is assigned before accessing it
            {
                houseDText.text = currentRoundRents[3].ToString() + "/Month";
            }
        }
        else
        {
            Debug.LogError("Invalid round index!");
        }
    }
}
