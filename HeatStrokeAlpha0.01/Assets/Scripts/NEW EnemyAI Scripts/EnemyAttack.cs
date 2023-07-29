using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackMelee
{
    public EnemyUnitScript thisUnit;
    public PlayerUnitScript pUnit;
    public AttackRangeFinder attackRangeFinder = new AttackRangeFinder();
    public List<HideAndShowScript> inRangeTiles = new List<HideAndShowScript>();

    public void Start()
    {
        /*
        inRangeTiles = new List<HideAndShowScript>();
        attackRangeFinder = new AttackRangeFinder();
        */
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
