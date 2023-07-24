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

    public bool markDefaultDeploy;
    public List<int> tileNumbersToMark;

    public GameObject dsPrefab;
    private GameObject ds_GO;

    private void Start()
    {
        Debug.Log("DeployMarker script starting");
        mapManager = FindObjectOfType<MapManager>();
        Debug.Log("MapManager object: " + mapManager);

        // Subscribe to the grid generated event
        GameEventSystem.current.onGridGenerated += MarkTilesWithDefenseStructures;
    }

    private void OnDestroy()
    {
        GameEventSystem.current.onGridGenerated -= MarkTilesWithDefenseStructures;

    }
    private void MarkTilesWithDefenseStructures()
    {

        overlayTiles = mapManager.GetOverLayTiles();


        foreach (int indexToMark in tileNumbersToMark)
        {
            if (indexToMark >= 0 && indexToMark < overlayTiles.Count)
            {
                KeyValuePair<Vector2Int, HideAndShowScript> tileEntry = overlayTiles[indexToMark];
                HideAndShowScript tile = tileEntry.Value;
                ds_GO = Instantiate(dsPrefab);
                PositionDefenseStructureOnTile(tile);
                tile.isBlocked = true;
            }
            else
            {
                Debug.LogWarning("Invalid index to mark: " + indexToMark);
            }
        }
    }

    private void PositionDefenseStructureOnTile(HideAndShowScript tile)
    {
        ds_GO.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z + 1);
        ds_GO.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 2;
    }
}