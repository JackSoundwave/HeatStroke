using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHeatValue(int heatValue, int currentValue)
    {
        slider.maxValue = heatValue;
        slider.value = currentValue;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHeatValue(int heatValue)
    {
        slider.value = heatValue;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
