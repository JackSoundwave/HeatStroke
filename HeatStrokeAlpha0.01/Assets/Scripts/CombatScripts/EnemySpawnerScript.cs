using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    /*
     * basic idea for this script, needs to be updated in the future to include random enemies, and instead to spawn a prefab called "enemyAboutToSpawn".
     * 
     * enemyAboutToSpawn is a prefab that selects it's own activeTile, sets that tile to "enemyAboutToSpawn = true", which prevents the spawner from creating a new prefab
     * on that specific tile.
     * 
     * When the enemyAboutToSpawn prefab dies, it spawns an enemyUnit from a random list of pre-determined prefabs.
    */

    [HideInInspector]
    public GameObject enemyUnitPrefab;

    [HideInInspector]
    public GameObject shooterPrefab;

    private MapManager mapManager;
    private List<KeyValuePair<Vector2Int, HideAndShowScript>> overlayTiles;

    public List<int> tileNumbersToMark;
    private List<Vector2Int> unblockedTiles;
    private int maxEnemies = 5;

    private void Start()
    {
        Debug.Log("EnemySpawnerScript starting");
        mapManager = FindObjectOfType<MapManager>();
        Debug.Log("MapManager object: " + mapManager);
        GameEventSystem.current.onGridGenerated += SpawnEnemyOnRandomTile;
        GameEventSystem.current.onEnemyDeath += SpawnEnemyOnRandomTile;
    }

    private void SpawnEnemyOnRandomTile()
    {
        Debug.Log("Spawning Enemies");
        overlayTiles = mapManager.GetOverLayTiles();
        unblockedTiles = new List<Vector2Int>();
        int counter = 0;

        foreach (var tile in overlayTiles)
        {
            //the counter variable is here so that it only counts the first 16 tiles. Meaning, the first two rows from the right hand side.
            //I hate living

            counter++;
            if (!tile.Value.isBlocked && counter <= 16)
            {
                unblockedTiles.Add(tile.Key);
            }
        }

        if (unblockedTiles.Count > 0)
        {
            int randomIndex = Random.Range(0, unblockedTiles.Count);
            Vector2Int randomTileKey = unblockedTiles[randomIndex];
            HideAndShowScript randomTile = overlayTiles.Find(tile => tile.Key == randomTileKey).Value;

            if (randomTile != null)
            {
                GameObject enemyUnit = Instantiate(enemyUnitPrefab, randomTile.transform.position, Quaternion.identity);
                EnemyUnitScript enemyUnitScript = enemyUnit.GetComponent<EnemyUnitScript>();
                enemyUnitScript.activeTile = randomTile;
                randomTile.isBlocked = true;
            }
            else
            {
                Debug.LogWarning("Selected tile is invalid. Trying again...");
                SpawnEnemyOnRandomTile();
            }
        }
        else
        {
            Debug.LogWarning("No available unblocked tiles. Enemy cannot be spawned.");
        }
    }

    private void markEnemySpawnTiles()
    {

    }

}
