using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnitSelectedState : PlayerUnitBaseState
{

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Player unit is selected!");
        //swaps the material of the sprite renderer to the "selected" material.
        player.thisUnit.GetComponent<SpriteRenderer>().material = player.thisUnit.selected;
    }

    public override void UpdateState(PlayerStateManager player)
    {

        if(player.thisUnit.isSelected == true)
        {
            if (player.thisUnit.hasAttacked == false && player.thisUnit.attackPrimed == true)
            {
                //swaps to "attackPrimedState" if the playerUnit is getting ready to attack.
                player.SwitchState(player.attackPrimedState);
            }
        } else if (player.thisUnit.isSelected == false)
        {
            player.SwitchState(player.idleState);
        }




        /*if(player.thisUnit.isSelected == true)
        {
            Debug.Log("We're still selected bois");
        } else if(player.thisUnit.isSelected == false)
        {
            player.SwitchState(player.idleState);
        }*/
    }

}
