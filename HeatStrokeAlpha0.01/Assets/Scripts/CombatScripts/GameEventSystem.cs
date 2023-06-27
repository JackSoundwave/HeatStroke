using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEventSystem : MonoBehaviour
{
    
    //Static reference of the current game event system so that it can be accessed from anywhere in the game / project file.
    public static GameEventSystem current;


    //Public list of units that currently holds info to be passed for all friendly units in the scene, when a unit is placed down, they're added into this public list by default via events.
    //It's so that when a unit dies, it gets removed from the list.
    //Keeps track of em basically.

    public PlayerUnitScript[] playerUnits = new PlayerUnitScript[3];



    private void Awake()
    {
        current = this;
    }

    //player related actions
    public event Action onUnitDeployed;
    public event Action onUnitSelected;
    public event Action onPlayerStartTurn;
    public event Action onPlayerEndTurn;
    public event Action onResetDeployPressed;

    //Enemy related actions
    public event Action onSpawnEnemies;

    //Map Related actions
    public event Action onGridGenerated;
    public event Action<MouseController> onMouseControllerCreated;

    //public Action<int> deez;

    public void generatedGrid()
    {
        Debug.Log("Grid Generation triggered");
        onGridGenerated?.Invoke();

        /*
         Using the line above, "onGridGenerated?.Invoke();" is fundamentally the same as using an if statement, kind of like how it is below.
        if (onGridGenerated != null)
        {
            onGridGenerated
            
        }
        The main difference is that it's apparently more cost effective. Idrk how, or why, but it apparently is.
        Jon says so, so it must be right. ig
         */
    }
    public void spawnEnemies()
    {
        Debug.Log("SpawnEnemies triggered");
        onSpawnEnemies?.Invoke();
    }

    public void deployUnit()
    {
        Debug.Log("Unit Deployed");
        onUnitDeployed?.Invoke();
    }

    public void mouseControllerCreated(MouseController mouseController)
    {
        Debug.Log("MouseController created");
        onMouseControllerCreated?.Invoke(mouseController);
    }

    public void playerTurnStarted()
    {
        Debug.Log("PlayerTurn started");
        onPlayerStartTurn?.Invoke();
    }

    public void unitSelected()
    {
        Debug.Log("Unit selected");
        onUnitSelected?.Invoke();
    }
    public void onPlayerTurnEnd()
    {
        Debug.Log("Player ended turn");
        onPlayerEndTurn?.Invoke();
    }
}
