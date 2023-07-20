using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class DeployMarker : MonoBehaviour
{
    public List<int> tileNumbersToMark; // Public list to hold tile numbers to mark as "Deploy Tile"

    private MapManager mapManager;
    private List<KeyValuePair<Vector2Int, HideAndShowScript>> overlayTiles;
    private List<Vector2Int> unblockedTiles;

    private void Start()
    {
        Debug.Log("DeployMarker script starting");
        mapManager = FindObjectOfType<MapManager>();
        Debug.Log("MapManager object: " + mapManager);

        // Subscribe to the grid generated event
        GameEventSystem.current.onGridGenerated += MarkTilesAsDeployArea;
    }

    private void MarkTilesAsDeployArea()
    {
        // Get the overlay tiles from the map manager
        overlayTiles = mapManager.GetOverLayTiles();

        // Loop through the overlayTiles and mark tiles as "Deploy Tile" based on the public list
        foreach (int indexToMark in tileNumbersToMark)
        {
            if (indexToMark >= 0 && indexToMark < overlayTiles.Count)
            {
                KeyValuePair<Vector2Int, HideAndShowScript> tileEntry = overlayTiles[indexToMark];
                HideAndShowScript tile = tileEntry.Value;

                tile.isDeployTile = true;
                tile.ShowTile();
                tile.DyeTileYellow();
            }
            else
            {
                Debug.LogWarning("Invalid index to mark: " + indexToMark);
            }
        }
    }
}