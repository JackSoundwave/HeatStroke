using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitScript : MonoBehaviour
{
    //Tile character is standing on
    public HideAndShowScript activeTile;

    //pretty sure I don't have to explain this
    public int health, movementRange, attackDmg, attackRange;

    //booleans to dictate whether or not the player has moved or has attacked already. canMove is set to true and hasAttacked is set to false.
    //If true, then the event manager tells the MouseController script that the unit can move after being selected.\
    //isAttacking is to determine the current pUnit state. If the unit is attacking, the selected tile afterwards gets an attack on it
    public bool canMove, hasAttacked, isAttacking;
    public bool isSelected;


    //Creating templates for the attack script so that i can use it later, might delete, idk
    public void PrimeWeapon()
    {
        isAttacking = true;
        Debug.Log("Weapon of" + gameObject.name + "is primed");
    }

    public void UnPrimeWeapon()
    {
        isAttacking = false;
        Debug.Log("Weapon of" + gameObject.name + "is unprimed");
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
