using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitScript : MonoBehaviour
{
    //Tile character is standing on
    public HideAndShowScript activeTile;

    //pretty sure I don't have to explain this
    public int health, movementRange, attackDmg, attackRange, maxHealth;

    //booleans to dictate whether or not the player has moved or has attacked already. canMove is set to true and hasAttacked is set to false.
    //If true, then the event manager tells the MouseController script that the unit can move after being selected.
    //isAttacking is to determine the current pUnit state. If the unit is attacking, the selected tile afterwards gets an attack on it
    public bool canMove, hasAttacked, isAttacking;
    public bool isSelected;
    public HealthBar healthBar;
    private void Start()
    {
        //upon starting, adds THIS unit to the EnemyUnit list in the GameEventSystem.
        GameEventSystem.current.enemyUnits.Add(this);
        healthBar.SetMaxHealth(maxHealth);
    }

    private void OnDestroy()
    {
        GameEventSystem.current.enemyUnits.Remove(this);
    }

    private void Update()
    {
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            GameEventSystem.current.enemyDeath();
            activeTile.isBlocked = false;
            Destroy(this.gameObject);
        }
    }
    public void PrimeWeapon()
    {
        isAttacking = true;
        Debug.Log("Weapon of" + gameObject.name + "is primed");
    }

    public void Attack(HideAndShowScript targetTile)
    {
        //attack logic is supposed to go here
        Debug.Log("Attacking Tile" + targetTile.name);

        //resetting attack state for future turns
        isAttacking = false;
        hasAttacked = true;
    }
}
