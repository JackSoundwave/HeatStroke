using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIsMovingState : PlayerUnitBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Unit is moving");
        player.thisUnit.GetComponent<SpriteRenderer>().material = player.thisUnit.normal;
        player.thisUnit.isMoving = true;
        player.cursor.HideInRangeTiles();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.cursor.getPath().Count > 0)
        {
            player.cursor.MoveAlongPath();
        } 
        else
        {
            player.thisUnit.canMove = false;
            player.thisUnit.isMoving = false;
            player.thisUnit.canMove = false;
            player.SwitchState(player.idleState);
        }
    }
}
