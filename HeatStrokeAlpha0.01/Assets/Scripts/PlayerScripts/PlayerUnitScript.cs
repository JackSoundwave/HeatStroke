using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnitScript : MonoBehaviour
{
    //Tile character is standing on
    public HideAndShowScript activeTile;

    //pretty sure I don't have to explain this
    public int health, movementRange, attackDmg, attackRange, maxHealth;

    //booleans to dictate whether or not the player has moved or has attacked already. canMove is set to true and hasAttacked is set to false.
    //If true, then the event manager tells the MouseController script that the unit can move after being selected.\
    //isAttacking is to determine the current pUnit state. If the unit is attacking, the selected tile afterwards gets an attack on it
    public bool canMove, hasAttacked, attackPrimed, isSelected, isAttacking, isMoving;
    public Material normal;
    public Material selected;

    private void Update()
    {
        if (health == 0)
        {
            killSelf();
        }
    }

    public void UnPrimeWeapon()
    {
        attackPrimed = false;
        Debug.Log("Weapon of" + gameObject.name + "is unprimed");
    }

    public void Attack(HideAndShowScript targetTile)
    {
        //attack logic is supposed to go here
        if(attackPrimed == true && hasAttacked == false)
        {
            isAttacking = true;
            
            Debug.Log("Attacking Tile " + targetTile.name);
            //attack function continues
            
        }
        else
        {
            
        }
    }
    
    private void killSelf()
    {
        Destroy(this);
    }
}
