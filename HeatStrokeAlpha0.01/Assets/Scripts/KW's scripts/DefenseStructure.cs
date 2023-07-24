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
        //so like, code to show the healthbar is supposed to go here, but I'm too lazy to program that shit rn
        //-Paven
    }

    private void OnMouseExit()
    {
        //lorem ipsum
    }
}
