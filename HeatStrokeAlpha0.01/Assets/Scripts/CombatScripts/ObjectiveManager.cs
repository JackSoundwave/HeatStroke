using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    //This is to keep track of objectives being completed/failed in the game. Also dictates what kind of mission it currently is.
    //We could also refactor it later to include side objectives.
    public static ObjectiveManager OMInstance;

    public Objective obj;

    public int hivesDestroyed, enemiesKilled, turnTimer;

    private void Awake()
    {
        OMInstance = this;
    }

    private void Start()
    {
      
    }

    //forgot how this method is supposed to work, give me 5 minutes to think of it.
    public void UpdateObjectiveDetails()
    {
        
    }

    //TurnTimer related (basically we need this for objectives that rely on the amount of turns going down to 0 for a win condition.
    //Creating this set method just in case we need it later.
    private void setTurnTimer(int turnTimer)
    {
        OMInstance.turnTimer = turnTimer;
    }

    //creating a getter method to use later, just in case.
    public int getTurnTimer()
    {
        return OMInstance.turnTimer;
    }

    public void evaluateTurnTimer()
    {
        if(OMInstance.turnTimer == 0)
        {
            if (OMInstance.obj == Objective.Defense)
            {
                CombatStateManager.CSInstance.UpdateCombatState(CombatState.Victory);
            }
        }
    }

    //okay real talk, is this method REALLY needed?
    private void setObjectiveToDefense()
    {
        OMInstance.obj = Objective.Defense;
    }

}

public enum Objective
{
    Extermination,
    Defense,
    Train,
    Escort,
}