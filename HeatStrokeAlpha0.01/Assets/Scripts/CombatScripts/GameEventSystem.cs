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

    public void generatedGrid()
    {
        Debug.Log("Grid Generation triggered");
        if (onGridGenerated != null)
        {
            onGridGenerated();
        }
    }
    public void spawnEnemies()
    {
        Debug.Log("SpawnEnemies triggered");
        if (onSpawnEnemies != null)
        {
            onSpawnEnemies();
        }
    }

    public void deployUnits()
    {
        Debug.Log("startPhase triggered");
        if(onUnitsDeployed != null)
        {
            onUnitsDeployed();
        }
    }
}
