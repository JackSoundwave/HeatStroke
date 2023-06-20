using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject enemyUnitPrefab;
    private MapManager mapManager;
    private List<KeyValuePair<Vector2Int, HideAndShowScript>> overlayTiles;
    private List<Vector2Int> unblockedTiles;

    private void Start()
    {
        Debug.Log("EnemySpawnerScript starting");
        mapManager = FindObjectOfType<MapManager>();
        Debug.Log("MapManager object: " + mapManager);
        GameEventSystem.current.onGridGenerated += SpawnEnemyOnRandomTile;
    }

    private void SpawnEnemyOnRandomTile()
    {
        Debug.Log("Spawning Enemies");
        overlayTiles = mapManager.GetOverLayTiles();
        unblockedTiles = new List<Vector2Int>();
        int counter = 0;

        foreach (var tile in overlayTiles)
        {
            // the counter variable is here so that it only counts the first 16 tiles. Meaning, the first two rows from the right hand side.
            // I hate living

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

}
