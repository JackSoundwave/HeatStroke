using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceStructure : MonoBehaviour
{
    public HideAndShowScript activeTile;
    public int maxHealth;
    public int currentHealth;

    public HeatGaugeSystem heatgaugeSystem;
    public HeatBar heatBar;
    public HealthBar healthBar;

    void Start()
    {
        heatBar = FindObjectOfType<HeatBar>();
        heatgaugeSystem = FindObjectOfType<HeatGaugeSystem>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }*/

        if (currentHealth <= 0)
        {
            DestroyStructure();
        }

        if (MouseController.ActiveInstance?.targetedDefenseStructure == this)
        {
            showHealthBar();
        }
        else
        {
            hideHealthBar();
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
        activeTile.isBlocked = false;
        activeTile.entity = null;
        Destroy(gameObject);
    }

    private void showHealthBar()
    {
        healthBar.gameObject.SetActive(true);
    }

    private void hideHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }
}
