using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatGaugeSystem : MonoBehaviour
{
    public int maxTemperature = 100;
    private int currentTemperature;


    public HeatBar heatBar;
    void Start()
    {
        currentTemperature = maxTemperature;
        heatBar.SetMaxHeatValue(maxTemperature);
    }

    // Update is called once per frame
    void Update()
    {
        //if i pressed the spacebar the potatooo will deducted 20hp
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage(int damage)
    {
        currentTemperature -= damage;

        heatBar.SetHeatValue(currentTemperature);
    }

    // Function to change the temperature
    public void ChangeTemperature(int heatValue)
    {
        currentTemperature += heatValue;

        // Ensure the current temperature stays within the acceptable range
        currentTemperature = Mathf.Clamp(currentTemperature, 0, maxTemperature);

        // Check if the temperature has reached critical levels
        if (currentTemperature >= maxTemperature)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        //tbc
    }
}
