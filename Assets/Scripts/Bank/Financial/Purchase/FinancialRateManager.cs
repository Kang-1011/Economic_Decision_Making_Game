// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinancialRateManager : MonoBehaviour
{
    public static FinancialRateManager Instance { get; private set; }

    private float averageRate_R1;
    private float averageRate_R3;
    private float averageRate_R5;
    private float currentRate_R1;
    private float currentRate_R3;
    private float currentRate_R5;

    private Queue<float> rates_R1 = new Queue<float>();
    private Queue<float> rates_R3 = new Queue<float>();
    private Queue<float> rates_R5 = new Queue<float>();

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
        Initialization();
    }

    public void Initialization()
    {
        for (int i = 0; i < 12; i++)
        {
            rates_R1.Enqueue(RandomRate_R1());
            rates_R3.Enqueue(RandomRate_R3());
            rates_R5.Enqueue(RandomRate_R5());
        }

        averageRate_R1 = CalculateAnnualRate(rates_R1);
        averageRate_R3 = CalculateAnnualRate(rates_R3);
        averageRate_R5 = CalculateAnnualRate(rates_R5);
    }

    // Generate the random price limit for R1
    private float RandomRate_R1()
    {
        return Random.Range(0.00175f, 0.00242f);
    }

    // Generate the random price limit for R3
    private float RandomRate_R3()
    {
        return Random.Range(-0.00631f, 0.01011f);
    }

    // Generate the random price limit for R5
    private float RandomRate_R5()
    {
        int upDown = Random.Range(1, 5);
        if (upDown <= 2)
        {
            return Random.Range(0.00000f, 0.04500f);
        }
        else
        {
            return Random.Range(-0.03150f, 0.00000f);
        }
    }

    private float CalculateAnnualRate(Queue<float> rates)
    {
        float sum = 0;
        foreach(float rate in rates)
        {
            sum += rate;
        }
        return sum;
    }

    public void UpdateRates()
    {
        currentRate_R1 = RandomRate_R1();
        currentRate_R3 = RandomRate_R3();
        currentRate_R5 = RandomRate_R5();

        rates_R1.Dequeue();
        rates_R3.Dequeue();
        rates_R5.Dequeue();

        rates_R1.Enqueue(currentRate_R1);
        rates_R3.Enqueue(currentRate_R3);
        rates_R5.Enqueue(currentRate_R5);

        averageRate_R1 = CalculateAnnualRate(rates_R1);
        averageRate_R3 = CalculateAnnualRate(rates_R3);
        averageRate_R5 = CalculateAnnualRate(rates_R5);
    }

    public float GetAverageRate_R1()
    {
        return averageRate_R1;
    }

    public float GetAverageRate_R3()
    {
        return averageRate_R3;
    }

    public float GetAverageRate_R5()
    {
        return averageRate_R5;
    }

    public float GetCurrentRate(string type)
    {
        if (type == "R1")
        {
            return currentRate_R1;
        }
        else if (type == "R3")
        {
            return currentRate_R3;
        }
        else
        {
            return currentRate_R5;
        }
    }
}
