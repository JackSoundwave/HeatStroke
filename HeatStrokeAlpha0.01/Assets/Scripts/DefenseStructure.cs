using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceStructure : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //if i pressed the spacebar the potatooo will deducted 20hp
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            DestroyStructure();
        }
    }
    private void DestroyStructure()
    {
        //DESTRYED WHEN HEALTH = 0
        Destroy(gameObject);
    }
<<<<<<< Updated upstream


=======
>>>>>>> Stashed changes
}
