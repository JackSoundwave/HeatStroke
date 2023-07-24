using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class DeadCellSpawner : MonoBehaviour
{
    public bool markDefaultDeploy;
    public List<int> tileNumbersToMark; 

    private MapManager mapManager;
    private List<KeyValuePair<Vector2Int, HideAndShowScript>> overlayTiles;

    private void Start()
    {
        Debug.Log("DeployMarker script starting");
        mapManager = FindObjectOfType<MapManager>();
        Debug.Log("MapManager object: " + mapManager);

        // Subscribe to the grid generated event
        GameEventSystem.current.onGridGenerated += MarkTilesAsDeployArea;
    }

    private void OnDestroy()
    {
        GameEventSystem.current.onGridGenerated -= MarkTilesAsDeployArea;
        
    }
    private void MarkTilesAsDeployArea()
    {
        
        overlayTiles = mapManager.GetOverLayTiles();

        
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
