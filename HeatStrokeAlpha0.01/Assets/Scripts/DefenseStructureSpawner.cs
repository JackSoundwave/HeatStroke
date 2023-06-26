using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseStructureSpawner : MonoBehaviour
{
    public GameObject defenseStructurePrefab;
    private MapManager mapManager;
    private List<KeyValuePair<Vector2Int, HideAndShowScript>> overlayTiles;
    private List<Vector2Int> unblockedTiles;
    private HideAndShowScript activeTile;

    private void Start()
    {
        Debug.Log("DefenseStructureSpawner starting");
        mapManager = FindObjectOfType<MapManager>();
        Debug.Log("MapManager object: " + mapManager);
        GameEventSystem.current.onGridGenerated += SpawnDefenseStructure;
    }

    private void SpawnDefenseStructure()
    {
        Debug.Log("Spawning DefenseStructure");
        overlayTiles = mapManager.GetOverLayTiles();
        unblockedTiles = new List<Vector2Int>();

        // Specify the coordinates of the desired tile
        int desiredX = 2;
        int desiredY = 3;
        Vector2Int desiredTileKey = new Vector2Int(desiredX, desiredY);

        if (overlayTiles.Exists(tile => tile.Key == desiredTileKey))
        {
            HideAndShowScript desiredTile = overlayTiles.Find(tile => tile.Key == desiredTileKey).Value;

            if (desiredTile != null && !desiredTile.isBlocked)
            {
                GameObject defenseStructure = Instantiate(defenseStructurePrefab, desiredTile.transform.position, Quaternion.identity);
                DefenseStructureSpawner defenseScript = defenseStructure.GetComponent<DefenseStructureSpawner>();
                defenseScript.activeTile = desiredTile;
                desiredTile.isBlocked = true;
            }
            else
            {
                Debug.LogWarning("The desired tile is already blocked or invalid. Defense structure cannot be spawned.");
            }
        }
        else
        {
            Debug.LogWarning("The desired tile is invalid. Defense structure cannot be spawned.");
        }
    }

}
