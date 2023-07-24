using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeatGaugeSystem : MonoBehaviour
{
    public static HeatGaugeSystem instance;
    public static int maxTemperature = 1000;
    public int currentTemperature = 0;
    public HeatBar heatBar;

    void Start()
    {
        heatBar = FindObjectOfType<HeatBar>();
        if(heatBar != null) 
        {
            heatBar.SetMaxHeatValue(maxTemperature, currentTemperature);
        }

        if(instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += onSceneChanged;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(heatBar != null) 
        {
            heatBar.SetHeatValue(currentTemperature);
        }

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

    private void onSceneChanged(Scene scene, LoadSceneMode mode)
    {
        heatBar = FindObjectOfType<HeatBar>();
    }
}
