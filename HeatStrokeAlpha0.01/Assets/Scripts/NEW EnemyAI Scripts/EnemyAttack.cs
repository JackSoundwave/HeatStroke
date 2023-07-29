using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAttack
{
    public EnemyUnitScript thisUnit, victim_E;
    public PlayerUnitScript victim_P;
    public DefenceStructure victim_D;

    public AttackRangeFinder attackRangeFinder = new AttackRangeFinder();
    public List<HideAndShowScript> inRangeTiles = new List<HideAndShowScript>();
    public HideAndShowScript targetTile;

    //Inherently the same to the "getInRangeTiles" for the movement, although this time, it's being used to get the attack range.
    public void getAttackRange()
    {
        inRangeTiles = attackRangeFinder.GetTilesInAttackRange(thisUnit.activeTile, thisUnit.attackRange);
        Debug.Log("Showing attack range");
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
        if(tileToAttack == attackRangeFinder.getAdjacentUpTile(thisUnit.activeTile))
        {
            thisUnit.SpawnUpAttackVFX(tileToAttack);
        }
        else if (tileToAttack == attackRangeFinder.getAdjacentDownTile(thisUnit.activeTile))
        {
            thisUnit.SpawnDownAttackVFX(tileToAttack);
        } 
        else if (tileToAttack == attackRangeFinder.getAdjacentRightTile(thisUnit.activeTile))
        {
            thisUnit.SpawnRightAttackVFX(tileToAttack);
        }
        else if(tileToAttack == attackRangeFinder.getAdjacentLeftTile(thisUnit.activeTile))
        {
            thisUnit.SpawnLeftAttackVFX(tileToAttack);
        }
        else
        {
            Debug.Log("Attack out of bounds? Look at EnemyAttack.cs");
        }
    }

    //returns true if the victim's tile is found in the list, returns false if otherwise.
    public bool checkVictim(List<HideAndShowScript> inRangeTiles, HideAndShowScript victimTile)
    {
        return inRangeTiles.Contains(victimTile);
    }

    public PlayerUnitScript findPlayerInAttackRange (List<HideAndShowScript> inRangeTiles)
    {
        inRangeTiles = returnAttackRange();
        foreach (HideAndShowScript tile in inRangeTiles)
        {
            if(tile.entity != null)
            {
                PlayerUnitScript victim = tile.entity.GetComponent<PlayerUnitScript>();
                Debug.Log("Victim found : " + victim);
                return victim;
            }
        }
        return null;
    }

    public DefenceStructure findDefenseStructureInRange(List<HideAndShowScript> inRangeTiles)
    {
        inRangeTiles = returnAttackRange();
        foreach (HideAndShowScript tile in inRangeTiles)
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
        Debug.Log("Executing attack");
        if (targetTile.entity != null)
        {
            if (victim_D == targetTile.entity.GetComponent<DefenceStructure>())
            {
                Debug.Log("Attacking structure");
                DamageDefenseStructure(victim_D);
            }
            else if (victim_P == targetTile.entity.GetComponent<PlayerUnitScript>())
            {
                Debug.Log("Attacking player");
                DamageEnemy(victim_P);
            }
            else if (targetTile.entity.GetComponent<EnemyUnitScript>() != null)
            {
                Debug.Log("Attacking enemy?");
                victim_E = targetTile.entity.GetComponent<EnemyUnitScript>();
                DamageAlly(victim_E);
            }
            else
            {
                Debug.Log("Unknown entity, not attacking");
            }
        }
    }
}
