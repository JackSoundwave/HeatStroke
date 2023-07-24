using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class DeadCellSpawner : MonoBehaviour
{
    public bool markDefaultDeploy;
    public List<int> tileNumbersToMark;

    public GameObject deadCellPrefab;
    private GameObject deadCell_GO;

    private MapManager mapManager;
    private List<KeyValuePair<Vector2Int, HideAndShowScript>> overlayTiles;

    private void Start()
    {
        Debug.Log("DeployMarker script starting");
        mapManager = FindObjectOfType<MapManager>();
        Debug.Log("MapManager object: " + mapManager);

        // Subscribe to the grid generated event
        GameEventSystem.current.onGridGenerated += MarkTilesWithDeadCells;
    }

    private void OnDestroy()
    {
        GameEventSystem.current.onGridGenerated -= MarkTilesWithDeadCells;
        
    }
    private void MarkTilesWithDeadCells()
    {
        
        overlayTiles = mapManager.GetOverLayTiles();

        
        foreach (int indexToMark in tileNumbersToMark)
        {
            if (indexToMark >= 0 && indexToMark < overlayTiles.Count)
            {
                KeyValuePair<Vector2Int, HideAndShowScript> tileEntry = overlayTiles[indexToMark];
                HideAndShowScript tile = tileEntry.Value;
                deadCell_GO = Instantiate(deadCellPrefab);
                PositionDeadCellOnTile(tile);
                tile.isBlocked = true;
            }
            else
            {
                Debug.LogWarning("Invalid index to mark: " + indexToMark);
            }
        }
    }

    private void PositionDeadCellOnTile(HideAndShowScript tile)
    {
        deadCell_GO.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z + 1);
        deadCell_GO.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 2;
    }
}
