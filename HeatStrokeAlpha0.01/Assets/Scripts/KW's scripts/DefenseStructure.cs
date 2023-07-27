using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceStructure : MonoBehaviour
{
    public HideAndShowScript activeTile;
    public int maxHealth = 2;
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
        Destroy(gameObject);
    }

    private void OnMouseOver()
    {
        healthBar.gameObject.SetActive(true);
        Debug.Log("It took me ten years to find");
    }

    private void OnMouseExit()
    {
        healthBar.gameObject.SetActive(false);
    }
}
