using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class EnemyAttack
{
    private EnemyUnitScript thisUnit, victim_E;
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

    public void executeAttackOnTile(HideAndShowScript targetTile)
    {
        Debug.Log("Executing attack");
        if(targetTile.entity != null) 
        {
            victim_D = targetTile.entity.GetComponent<DefenceStructure>();
            victim_E = targetTile.entity.GetComponent<EnemyUnitScript>();
            victim_P = targetTile.entity.GetComponent<PlayerUnitScript>();

            if (victim_D != null)
            {
                //play attack vfx
                Debug.Log("Attacking structure");
                DamageDefenseStructure(victim_D);
            }
            else if (victim_P != null)
            {
                //play attack vfx
                Debug.Log("Attacking player");
                DamageEnemy(victim_P);
            }
            else if (victim_E != null)
            {
                //just read up okay
                Debug.Log("Attacking enemy?");
                DamageAlly(victim_E);
            }
            else
            {
                // play attack vfx, same as usual
                Debug.Log("Unknown entity");
            }
        }        
    }

    public void setThisUnit(EnemyUnitScript unit)
    {
        thisUnit = unit;
    }

    public EnemyUnitScript getThisUnit()
    {
        return thisUnit;
    }
}
