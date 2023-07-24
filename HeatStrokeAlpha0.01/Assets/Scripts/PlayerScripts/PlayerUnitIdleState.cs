using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnitIdleState : PlayerUnitBaseState
{

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Player unit is now idle");
        //Swapping material to "idle" material.
        player.thisUnit.GetComponent<SpriteRenderer>().material = player.thisUnit.normal;
        if(player.cursor != null) 
        {
            if(CombatStateManager.CSInstance.State != CombatState.DeployPhase)
            {
                player.cursor.HideInRangeTiles();
            }
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.thisUnit.isSelected == true)
        {
           player.SwitchState(player.selectedState);
        }
    }    
}
