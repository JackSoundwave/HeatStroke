using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseStructureSpawner : MonoBehaviour
{
    public GameObject DefenseStructureUnitPrefab;
    private MapManager mapManager;
    private List<KeyValuePair<Vector2Int, HideAndShowScript>> overlayTiles;
    private List<Vector2Int> unblockedTiles;

    private void Start()
    {
        Debug.Log("DefenseStructureScript starting");
        mapManager = FindObjectOfType<MapManager>();
        Debug.Log("MapManager object: " + mapManager);
        GameEventSystem.current.onGridGenerated += SpawnDefenseStructure;
    }

    private void SpawnDefenseStructure()
    {
        Debug.Log("Spawning Defense Structure");
        overlayTiles = mapManager.GetOverLayTiles();
        unblockedTiles = new List<Vector2Int>();

        // Iterate through the overlay tiles and find unblocked tiles in the first two rows from the right side
        int counter = 0;
        foreach (var tile in overlayTiles)
        {
            counter++;
            if (!tile.Value.isBlocked && counter <= 16)
            {
                unblockedTiles.Add(tile.Key);
            }
        }

        if (unblockedTiles.Count > 0)
        {
            Vector2Int desiredTileKey = new Vector2Int(-3, -6);
            float tileSize = mapManager.GetTileSize(); // Get the size of the tiles from the mapManager (assuming it provides this information)
            Vector3 desiredTilePosition = new Vector3(desiredTileKey.x + 0.5f, desiredTileKey.y + 0.5f, 0f) * tileSize;

            HideAndShowScript desiredTile = overlayTiles.Find(tile => tile.Key == desiredTileKey).Value;

            if (desiredTile != null && !desiredTile.isBlocked)
            {
                Debug.Log("Defense Structure Spawned");
                GameObject defenseStructureUnit = Instantiate(DefenseStructureUnitPrefab, desiredTilePosition, Quaternion.identity);
                DefenceStructure defenseStructure = defenseStructureUnit.GetComponent<DefenceStructure>();
                defenseStructure.activeTile = desiredTile;
                desiredTile.isBlocked = true;
            }
            else
            {
                Debug.LogWarning("The desired tile is already blocked or invalid. Defense structure cannot be spawned.");
            }


        }
        else
        {
            Debug.LogWarning("No available unblocked tiles. Defense structure cannot be spawned.");
        }
    }
}
