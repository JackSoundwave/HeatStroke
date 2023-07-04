using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceStructure : MonoBehaviour
{
    public HideAndShowScript activeTile;
    public int maxHealth = 100;
    public int currentHealth;

    public HeatGaugeSystem heatgaugeSystem;
    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }

        if (currentHealth <= 0)
        {
            DestroyStructure();
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    private void DestroyStructure()
    {
        heatgaugeSystem.IncreaseTemperature(100); // Increase the temperature by 100
        Destroy(gameObject);
    }
}
