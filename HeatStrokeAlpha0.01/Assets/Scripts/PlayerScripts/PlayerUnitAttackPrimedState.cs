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
        if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0))
        {
            player.thisUnit.attackPrimed = false;
            player.playerAttack.HideInRangeTiles();
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

    // Start is called before the first frame update
    public void UnPrimeWeapon(PlayerStateManager player)
    {
        player.thisUnit.attackPrimed = false;
        Debug.Log("Weapon of" + player.thisUnit.gameObject.name + "is unprimed");
    }
}
