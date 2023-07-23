using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class DeployMarker : MonoBehaviour
{
    public bool markDefaultDeploy;
    public List<int> tileNumbersToMark; // Public list to hold tile numbers to mark as "Deploy Tile"
    private List<int> defaultZone;

    private MapManager mapManager;
    private List<KeyValuePair<Vector2Int, HideAndShowScript>> overlayTiles;

    private void Start()
    {
        Debug.Log("DeployMarker script starting");
        mapManager = FindObjectOfType<MapManager>();
        Debug.Log("MapManager object: " + mapManager);

        // Subscribe to the grid generated event
        GameEventSystem.current.onGridGenerated += MarkTilesAsDeployArea;
        if(markDefaultDeploy == true)
        {
            GameEventSystem.current.onGridGenerated += MarkDefaultDeployZone;
        }
    }

    private void OnDestroy()
    {
        GameEventSystem.current.onGridGenerated -= MarkTilesAsDeployArea;
        if(markDefaultDeploy == true)
        {
            GameEventSystem.current.onGridGenerated -= MarkDefaultDeployZone;
        }
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

    private void MarkDefaultDeployZone()
    {
        defaultZone = new List<int> { 46, 45, 44, 43, 42, 41, 38, 37, 36, 35, 34, 33, 30, 29, 28, 27, 26, 25};
        overlayTiles = mapManager.GetOverLayTiles();

        foreach (int indexToMark in defaultZone)
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
                Debug.LogWarning("Default Zone could not be resolved");
            }
        }
    }
}