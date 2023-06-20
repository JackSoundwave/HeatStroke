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

    public event Action onSpawnEnemies;
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
}
