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
    public List<int> tileNumbersToMark;

    public GameObject dsPrefab;
    private GameObject ds_GO;

    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
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
                ds_GO.GetComponent<DefenceStructure>().activeTile = tile;
                PositionDefenseStructureOnTile(tile);
                tile.isBlocked = true;
                ds_GO = null;
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