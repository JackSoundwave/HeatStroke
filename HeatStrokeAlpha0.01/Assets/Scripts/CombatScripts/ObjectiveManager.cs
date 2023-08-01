using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    //This is to keep track of objectives being completed/failed in the game. Also dictates what kind of mission it currently is.
    //We could also refactor it later to include side objectives.
    public static ObjectiveManager OMInstance;

    [HideInInspector]
    public Objective obj;

    [HideInInspector]
    public int hivesToDestroy, enemiesKilled, turnTimer, PoCKillCounter;

    private void OnDestroy()
    {
        GameEventSystem.current.onEnemyTurnEnd -= OMInstance.reduceTurnTimer;
        GameEventSystem.current.onExterminateStructureDeath -= OMInstance.reduceHivesToDestroy;
        GameEventSystem.current.onEnemyDeath -= OMInstance.reduceEnemiesToKill;
    }

    //Initialize objective type and set up subscriptions to game event within the start() function.
    private void Start()
    {
        OMInstance = this;
    }

    public void setObjective(Objective newObj)
    {
        obj = newObj;
        switch (OMInstance.obj)
        {
            case Objective.Defense:
                GameEventSystem.current.onEnemyTurnEnd += OMInstance.reduceTurnTimer;
                break;

            case Objective.Extermination:
                GameEventSystem.current.onEnemyTurnEnd += OMInstance.reduceTurnTimer;
                GameEventSystem.current.onExterminateStructureDeath += OMInstance.reduceHivesToDestroy;

                break;

            case Objective.Escort:
                GameEventSystem.current.onEnemyTurnEnd += OMInstance.reduceTurnTimer;
                //tba
                break;

            case Objective.PoC_Extermination:
                GameEventSystem.current.onEnemyDeath += OMInstance.reduceEnemiesToKill;
                break;
            default:
                Debug.Log("ERROR: MORON DID NOT SET PROPER OBJECTIVE TYPE");
                throw new ArgumentOutOfRangeException(nameof(OMInstance.obj), OMInstance.obj, null);
        }
    }

    public void evaluateWinCondition()
    {
        switch (OMInstance.obj)
        {
            case Objective.Defense:
                if(OMInstance.turnTimer <= 0)
                {
                    CombatStateManager.CSInstance.UpdateCombatState(CombatState.Victory);
                } 
                else
                {
                    CombatStateManager.CSInstance.UpdateCombatState(CombatState.PlayerTurn);
                }
                break;

            case Objective.Extermination:
                if (OMInstance.turnTimer <= 0 || OMInstance.hivesToDestroy <= 0)
                {
                    CombatStateManager.CSInstance.UpdateCombatState(CombatState.Victory);
                }
                else
                {
                    CombatStateManager.CSInstance.UpdateCombatState(CombatState.PlayerTurn);
                }
                break;

            case Objective.Escort:
                if (OMInstance.turnTimer <= 0)
                {
                    CombatStateManager.CSInstance.UpdateCombatState(CombatState.Victory);
                }
                else
                {
                    CombatStateManager.CSInstance.UpdateCombatState(CombatState.PlayerTurn);
                }
                break;

            case Objective.PoC_Extermination:
                if (OMInstance.PoCKillCounter <= 0)
                {
                    CombatStateManager.CSInstance.UpdateCombatState(CombatState.Victory);
                }
                else
                {
                    CombatStateManager.CSInstance.UpdateCombatState(CombatState.PlayerTurn);
                }
                break;

            default:
                Debug.Log("ERROR: MORON DID NOT SET PROPER OBJECTIVE TYPE");
                throw new ArgumentOutOfRangeException(nameof(OMInstance.obj), OMInstance.obj, null);
        }
    }

    //TurnTimer related (basically we need this for objectives that rely on the amount of turns going down to 0 for a win condition.
    //Creating this set method just in case we need it later.
    public void setTurnTimer(int turnTimer)
    {
        OMInstance.turnTimer = turnTimer;
    }

    //creating a getter method to use later, just in case.
    public int getTurnTimer()
    {
        return OMInstance.turnTimer;
    }

    //Method to reduce turn timer by 1 everytime the enemy's turn ends and then evaluate win condition afterwards.
    public void reduceTurnTimer()
    {
        OMInstance.turnTimer = OMInstance.turnTimer - 1;
        evaluateWinCondition();
    }

    public void setHivesToDestroy(int hives)
    {
        OMInstance.hivesToDestroy = hives;
    }

    public void setPoCKillCount(int kills)
    {
        PoCKillCounter = kills;
    }
    public void reduceHivesToDestroy()
    {
        OMInstance.hivesToDestroy = OMInstance.hivesToDestroy - 1;
        evaluateWinCondition();
    }

    public void reduceEnemiesToKill()
    {
        OMInstance.PoCKillCounter = OMInstance.PoCKillCounter - 1;
        evaluateWinCondition();
    }
}

public enum Objective
{
    Extermination,
    Defense,
    Escort,
    PoC_Extermination
}