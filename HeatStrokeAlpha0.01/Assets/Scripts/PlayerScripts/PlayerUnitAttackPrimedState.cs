using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitAttackPrimedState : PlayerUnitBaseState
{

    public override void EnterState(PlayerStateManager player)
    {

    }

    public override void UpdateState(PlayerStateManager player)
    {

    }
    // Start is called before the first frame update

    public void UnPrimeWeapon(PlayerStateManager player)
    {
        player.thisUnit.attackPrimed = false;
        Debug.Log("Weapon of" + player.thisUnit.gameObject.name + "is unprimed");
    }
}
