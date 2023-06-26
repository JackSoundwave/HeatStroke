using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatGaugeSystem : MonoBehaviour
{
    public static int maxTemperature = 1000;
    public int currentTemperature = 0;

    public HeatBar heatBar;

    void Start()
    {
        heatBar.SetMaxHeatValue(maxTemperature, currentTemperature);
    }

    void Update()
    {
        heatBar.SetHeatValue(currentTemperature);

        currentTemperature = Mathf.Clamp(currentTemperature, 0, maxTemperature);

        if (currentTemperature >= maxTemperature)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        // Handle game over logic here
    }

    public void IncreaseTemperature(int temperature)
    {
        currentTemperature += temperature;
        currentTemperature = Mathf.Clamp(currentTemperature, 0, maxTemperature);
        heatBar.SetHeatValue(currentTemperature);
    }
}
