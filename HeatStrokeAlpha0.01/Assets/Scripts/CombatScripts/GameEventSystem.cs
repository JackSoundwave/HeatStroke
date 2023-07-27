using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameEventSystem : MonoBehaviour
{
    
    //Static reference of the current game event system so that it can be accessed from anywhere in the game / project file.
    public static GameEventSystem current;


    //Public array of units that currently holds info to be passed for all friendly units in the scene, when a unit is placed down, they're added into this public list by default via events.
    //It's so that when a unit dies, it gets removed from the list.
    //Keeps track of em basically.

    //unitsToDeploy acts as the holder for the player's currently selected team. Hence, they're a GameObject that's stored.
    [HideInInspector]
    public GameObject[] unitsToDeploy = new GameObject[3];

    [HideInInspector]
    //These are the playerUnits currently on the field, resets after every level.
    public GameObject[] playerUnits = new GameObject[3];

    //public list of enemyUnits to get all the enemyUnits in the scene. We need this so that we can iterate through the list and decide which units act first.
    //The first unit in this list acts first, then the next unit acts.
    //Once all units have acted, cycle back to player turn.
    public List<GameObject> enemyUnits;



    private void Awake()
    {
        current = this;
        SceneManager.sceneLoaded += onSceneLoaded; 
    }

    //==Player Related Actions==//
    public event Action onUnitSpawned;
    public event Action onUnitDeployed;
    public event Action onUnitSelected;
    public event Action onPlayerStartTurn;
    public event Action onPlayerEndTurn;
    public event Action onConfirmDeployPressed;
    public event Action onResetDeployPressed;
    public event Action onPrimeAttackButtonPressed;
    //==Player Related Actions==//

    //==Enemy Related Actions==//
    public event Action onSpawnEnemies;
    public event Action onEnemyTurnStart;
    public event Action onEnemyTurnEnd;
    public event Action onEnemyDeath;
    //==Enemy Related Actions==//

    //==Map Related actions==//
    public event Action onGridGenerated;
    public event Action<MouseController> onMouseControllerCreated;
    //==Map Related actions==//

    //==Objective Related actions==//
    public event Action onDefenseStructureDeath;
    public event Action onExterminateStructureDeath;
    //==Objective Related actions==//


    //==Misc==//
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
    public void mouseControllerCreated(MouseController mouseController)
    {
        Debug.Log("MouseController created");
        onMouseControllerCreated?.Invoke(mouseController);
    }
    //==Misc==//



    //==Enemy Related==//
    public void spawnEnemies()
    {
        Debug.Log("SpawnEnemies triggered");
        onSpawnEnemies?.Invoke();
    }

    public void enemyTurnStart()
    {
        Debug.Log("enemyTurnStart triggered");
        onEnemyTurnStart?.Invoke();
    }

    public void enemyTurnEnd()
    {
        Debug.Log("enemyTurnEnd triggered");
        onEnemyTurnEnd?.Invoke();
    }
    public void enemyDeath()
    {
        Debug.Log("Enemy died");
        onEnemyDeath?.Invoke(); //tysm Jon
    }
    //==Enemy Related==//



    //==Player Related==//
    public void spawnUnit()
    {
        Debug.Log("Unit spawned");
        onUnitSpawned?.Invoke();
    }
    public void deployUnit()
    {
        Debug.Log("Unit Deployed");
        onUnitDeployed?.Invoke();
    }
    public void playerTurnStarted()
    {
        //Debug.Log("PlayerTurn started");
        onPlayerStartTurn?.Invoke();
    }
    public void unitSelected()
    {
        Debug.Log("Unit selected");
        onUnitSelected?.Invoke();
    }
    public void playerEndTurn()
    {
        Debug.Log("Player ended turn");
        onPlayerEndTurn?.Invoke();
    }
    public void resetDeployPressed()
    {
        Debug.Log("Player reset deploy");
        onResetDeployPressed?.Invoke();
        resetDeploy();
    }
    public void confirmDeployPressed()
    {
        Debug.Log("Player Confirmed Deploy");
        onConfirmDeployPressed?.Invoke();

    }
    public void primeAttackButtonPressed()
    {
        Debug.Log("Priming Attack");
        onPrimeAttackButtonPressed?.Invoke();
    }
    //==Player Related==//



    //==Objective Related==//
    public void defenseStructureDeath()
    {
        Debug.Log("Defense structure has been destroyed");
        onDefenseStructureDeath?.Invoke();
    }
    public void exterminateStructureDeath()
    {
        Debug.Log("Hive exterminated");
        onExterminateStructureDeath?.Invoke();
    }
    //==Objective Related==//

    //==Changing Scene Related==//
    private void resetLists()
    {
        //resetting unitList
        for (int i =0; i < playerUnits.Length; i++)
        {
            playerUnits[i] = null;
        }
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        resetLists();
    }
    //==Changing Scene Related==//

    public void resetDeploy()
    {
        //Code should execute in this order:
        //Kill all playerUnits on the grid (Only the PlayerUnits that are part of the players team. This is an important distinction to make.)
        //Reset the MouseController's deploy unit list
        foreach (GameObject playerUnit in GameEventSystem.current.playerUnits)
        {
            if (playerUnit != null)
            {
                playerUnit.gameObject.GetComponent<PlayerUnitScript>()?.removeSelfFromList();
                playerUnit.gameObject.GetComponent<PlayerUnitScript>()?.activeTile.unBlockSelf();
                Destroy(playerUnit);
            }
        }
    }
}
