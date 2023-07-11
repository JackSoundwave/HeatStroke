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
            Vector2Int desiredTileKey = new Vector2Int(-3, -6); // Set desiredX and desiredY to the desired tile's coordinates

            HideAndShowScript desiredTile = overlayTiles.Find(tile => tile.Key == desiredTileKey).Value;

            if (desiredTile != null && !desiredTile.isBlocked)
            {
                Debug.Log("DefenseStructure Spawned");
                GameObject defenseStructureUnit = Instantiate(DefenseStructureUnitPrefab, desiredTile.transform.position, Quaternion.identity);
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
            Debug.LogWarning("No available unblocked tiles. Enemy cannot be spawned.");
        }
    }

}
