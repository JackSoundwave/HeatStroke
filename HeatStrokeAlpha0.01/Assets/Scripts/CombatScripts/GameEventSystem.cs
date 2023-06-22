using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEventSystem : MonoBehaviour
{

    public static GameEventSystem current;

    private void Awake()
    {
        current = this;
    }
    //player Turn related actions
    public event Action onUnitsDeployed;

    //Enemy Turn related actions
    public event Action onSpawnEnemies;

    //Map Related actions
    public event Action onGridGenerated;

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
        It also serves the dual purpose of decoupling code so that it's more modular and easier to work with. :)
         */
    }
    public void spawnEnemies()
    {
        Debug.Log("SpawnEnemies triggered");
        onSpawnEnemies?.Invoke();
    }

    public void deployUnits()
    {
        Debug.Log("startPhase triggered");
        onUnitsDeployed?.Invoke();
    }

}
