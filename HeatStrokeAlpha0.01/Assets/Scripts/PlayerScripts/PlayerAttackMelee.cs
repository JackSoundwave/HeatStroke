using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerAttackMelee : MonoBehaviour
{

    /*
     Pseudocode notes

    GetAttackTilesInRange(); <-- using rangefinder
    use rangefinder reference
    get a reference to any script using "EnemyUnitScript", so we have to import that too
    get a reference to PlayerUnitScript, obviously
    PrimeAttack(); function
    Change Player state to "attack primed" and then set "canMove" to false if so.
    Attack cancelled? ->> set "canMove" back to true.
    Attack followed through? ->> set "canMove" to false, set "canAttack" to false. Player cannot move after attacking, this is part of the design.

    Attack should somehow push enemy unit back one tile away, need to get enemyActiveTile and then somehow get it's neighbouring tiles, then decide how to move the enemy after that.
    Attacks should be directional. Need to do some math in regards to enemypos and playerpos. Should take a look at the ManHattan distance formula and do some math to find the respective tile.

    Math has something to do with playerActiveTile and enemyActiveTile. Something to do with top tile, bottom tile, right tile, and left tile.
     */

    public EnemyUnitScript victim;
    public PlayerUnitScript pUnit;

    private AttackRangeFinder attackRangeFinder;
    private List<HideAndShowScript> inRangeTiles;

    public void Start()
    {

        inRangeTiles = new List<HideAndShowScript>();
        attackRangeFinder = new AttackRangeFinder();
        pUnit = GetComponent<PlayerUnitScript>();

    }



    //Inherently the same to the "getInRangeTiles" for the movement, although this time, it's being used to get the attack range.
    private void getAttackRange()
    {
        inRangeTiles = attackRangeFinder.GetTilesInAttackRange(pUnit.activeTile, pUnit.attackRange);
    }
    
    private void DamageEnemy(EnemyUnitScript victim)
    {
        victim.health = victim.health - 1;
    }

}
