using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerAttack
{
    /*
     !!Pseudocode notes!!

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
    public AttackRangeFinder attackRangeFinder;
    public List<HideAndShowScript> inRangeTiles;

    public void Start()
    {
        inRangeTiles = new List<HideAndShowScript>();
        attackRangeFinder = new AttackRangeFinder();
    }



    //Inherently the same to the "getInRangeTiles" for the movement, although this time, it's being used to get the attack range.
    public void getAttackRange()
    {
        inRangeTiles = attackRangeFinder.GetTilesInAttackRange(pUnit.activeTile, pUnit.attackRange);

        foreach (var item in inRangeTiles)
        {
            ShowInRangetiles();
        }
    }

    //Inherently the same to the "getInRangeTiles" for the movement, although this time, it's being used to get the attack range.
    public List<HideAndShowScript> returnAttackRange()
    {
        inRangeTiles = attackRangeFinder.GetTilesInAttackRange(pUnit.activeTile, pUnit.attackRange);
        return inRangeTiles;
    }

    public void DamageEnemy(EnemyUnitScript victim)
    {
        victim.health = victim.health - pUnit.attackDmg;
    }

    public void ShowInRangetiles()
    {
        inRangeTiles = attackRangeFinder.GetTilesInAttackRange(pUnit.activeTile, pUnit.attackRange);
        foreach (var item in inRangeTiles)
        {
            item.ShowTile();
            item.DyeTileRed();
        }
    }

    public void HideInRangeTiles()
    {
        inRangeTiles = attackRangeFinder.GetTilesInAttackRange(pUnit.activeTile, pUnit.attackRange);
        foreach (var item in inRangeTiles)
        {
            item.HideTile();
        }
    }

    //returns true if the victim's tile is found in the list, returns false if otherwise.
    public bool findVictim(List<HideAndShowScript> inRangeTiles, HideAndShowScript victimTile)
    {
        return inRangeTiles.Contains(victimTile);
    }

}
