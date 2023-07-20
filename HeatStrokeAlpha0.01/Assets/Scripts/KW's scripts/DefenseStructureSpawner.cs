using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        GameEventSystem.current.onGridGenerated += LevelDeclaration;
    }

    private void LevelDeclaration()
    {
        // Get the current level or scene name (you may need to implement this based on how your game handles levels/scenes)
        string currentLevel = SceneManager.GetActiveScene().name;

        // Call the appropriate method to spawn DefenseStructures based on the current level
        switch (currentLevel)
        {
            case "Level1":
                SpawnDefenseStructuresLevel1();
                break;
            case "Level2":
                SpawnDefenseStructuresLevel2();
                break;
            // Add more cases for other levels as needed
            default:
                // If the current level is not defined in the switch cases, do nothing or handle the situation accordingly.
                break;
        }
    }

    private void SpawnDefenseStructuresLevel1()
    {
        // Define the coordinates for level 1's DefenseStructures
        Vector2Int desiredTileKey1 = new Vector2Int(-3, -6);
        Vector2Int desiredTileKey2 = new Vector2Int(-4, -7);
        Vector2Int desiredTileKey3 = new Vector2Int(-5, -8);

        // Call the SpawnDefenseStructure method for each desired tile key
        SpawnDefenseStructure(desiredTileKey1);
        SpawnDefenseStructure(desiredTileKey2);
        SpawnDefenseStructure(desiredTileKey3);
    }

    private void SpawnDefenseStructuresLevel2()
    {
        // Define the coordinates for level 2's DefenseStructures
        Vector2Int desiredTileKey1 = new Vector2Int(1, -3);
        Vector2Int desiredTileKey2 = new Vector2Int(2, -4);

        // Call the SpawnDefenseStructure method for each desired tile key
        SpawnDefenseStructure(desiredTileKey1);
        SpawnDefenseStructure(desiredTileKey2);
    }

    private void SpawnDefenseStructure(Vector2Int desiredTileKey)
    {
        overlayTiles = mapManager.GetOverLayTiles();
        unblockedTiles = new List<Vector2Int>();
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
            desiredTileKey = new Vector2Int(-3, -6); // Set desiredX and desiredY to the desired tile's coordinates

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