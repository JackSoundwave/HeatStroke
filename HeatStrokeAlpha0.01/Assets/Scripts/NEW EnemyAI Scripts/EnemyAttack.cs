using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAttackMelee
{
    public EnemyUnitScript thisUnit, victim_E;
    public PlayerUnitScript victim_P;
    public DefenceStructure victim_D;

    public AttackRangeFinder attackRangeFinder = new AttackRangeFinder();
    public List<HideAndShowScript> inRangeTiles = new List<HideAndShowScript>();
    public HideAndShowScript targetTile;

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
        inRangeTiles = attackRangeFinder.GetTilesInAttackRange(thisUnit.activeTile, thisUnit.attackRange);

        foreach (var item in inRangeTiles)
        {
            ShowInRangetiles();
        }
    }

    //Inherently the same to the "getInRangeTiles" for the movement, although this time, it's being used to get the attack range.
    public List<HideAndShowScript> returnAttackRange()
    {
        inRangeTiles = attackRangeFinder.GetTilesInAttackRange(thisUnit.activeTile, thisUnit.attackRange);
        return inRangeTiles;
    }

    public void DamageEnemy(PlayerUnitScript victim)
    {
        victim.health = victim.health - thisUnit.attackDmg;
    }

    public void DamageAlly(EnemyUnitScript victim)
    {
        victim.health = victim.health - thisUnit.attackDmg;
    }

    public void DamageDefenseStructure(DefenceStructure victim)
    {
        victim.currentHealth = victim.currentHealth - thisUnit.attackDmg;
    }

    public void ShowInRangetiles()
    {
        inRangeTiles = attackRangeFinder.GetTilesInAttackRange(thisUnit.activeTile, thisUnit.attackRange);
        foreach (var item in inRangeTiles)
        {
            item.ShowTile();
            item.DyeTileRed();
        }
    }

    public void HideInRangeTiles()
    {
        inRangeTiles = attackRangeFinder.GetTilesInAttackRange(thisUnit.activeTile, thisUnit.attackRange);
        foreach (var item in inRangeTiles)
        {
            item.HideTile();
        }
    }

    public void primeAttackOnTile(HideAndShowScript tileToAttack)
    {
        tileToAttack.DyeTileRed();
    }

    //returns true if the victim's tile is found in the list, returns false if otherwise.
    public bool checkVictim(List<HideAndShowScript> inRangeTiles, HideAndShowScript victimTile)
    {
        return inRangeTiles.Contains(victimTile);
    }

    public PlayerUnitScript findPlayerInAttackRange (List<HideAndShowScript> inRangetiles)
    {
        foreach (HideAndShowScript tile in inRangeTiles)
        {
            if(tile.entity != null)
            {
                PlayerUnitScript victim = tile.entity.GetComponent<PlayerUnitScript>();
                return victim;
            }
        }
        return null;
    }

    public DefenceStructure findDefenseStructureInRange(List<HideAndShowScript> inRangeTiles)
    {
        foreach(HideAndShowScript tile in inRangeTiles)
        {
            if(tile.entity != null)
            {
                DefenceStructure victim = tile.entity.GetComponent<DefenceStructure>();
                return victim;
            }
        }
        return null;
    }

    public void executeAttackOnTile(HideAndShowScript targetTile)
    {
        if (targetTile.entity != null)
        {
            if (victim_D == targetTile.entity.GetComponent<DefenceStructure>())
            {
                DamageDefenseStructure(victim_D);
            }
            else if (victim_P == targetTile.entity.GetComponent<PlayerUnitScript>())
            {
                DamageEnemy(victim_P);
            }
            else if (targetTile.entity.GetComponent<EnemyUnitScript>() != null)
            {
                victim_E = targetTile.entity.GetComponent<EnemyUnitScript>();
                DamageAlly(victim_E);
            }
        }
    }
}
