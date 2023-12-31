using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitAttackPrimedState : PlayerUnitBaseState
{

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entering attack primed state");
        player.cursor.HideInRangeTiles();
        player.playerAttack.getAttackRange();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        //constantly updates to change the current "victim" variable in the playerAttack script to the targetedEnemy in the cursor. Essentially where the player hovers.
        player.playerAttack.victim = player.cursor.targetedEnemyUnit;



        if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1))
        {
            player.thisUnit.attackPrimed = false;
            player.thisUnit.isSelected = false;
            player.playerAttack.HideInRangeTiles();
            player.SwitchState(player.idleState);
        }

        else if (Input.GetMouseButtonDown(0))
        {
            try
            {
                //essentially, if the victim is found, executes order66 on them
                Debug.Log("player.cursor.targetedEnemyUnit is " + player.cursor.targetedEnemyUnit);
                //this line basically says "if the enemy is within the attack range when the player clicked, attack that sumbitch"
                if (player.playerAttack.findVictim(player.playerAttack.returnAttackRange(), player.playerAttack.victim?.activeTile) && player.cursor.targetedEnemyUnit != null)
                {
                    player.thisUnit.hasAttacked = true;
                    player.thisUnit.canMove = false;
                    player.playerAttack.DamageEnemy(player.playerAttack.victim);
                    player.playerAttack.HideInRangeTiles();
                    player.SwitchState(player.idleState);
                }
                else
                {
                    Debug.Log("invalid target");
                }
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e);
            }
        }
    }
}
