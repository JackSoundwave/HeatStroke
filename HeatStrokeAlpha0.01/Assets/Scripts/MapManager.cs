using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;


/*This script will handle everything related to map logic using the HideAndShowScript tiles. Just a heads up, the "HideAndShowScript" class or .cs file is essentially the main code for the gridtiles 
 in the scene currently.
So if we want to change the map generation logic, we do so here. Adding new preset maps, adding a river, etc. all of it is done here.
*/
public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager instance2 { get { return _instance; } }

    public HideAndShowScript overlayTilePrefab;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, HideAndShowScript> map;

    //This dictionary is here to prevent issues where the tile selected is "under" another tile.
    //It also helps prevent the cursor appearing on an object that is currently highlighted.
    //The cursor meaning the orange sprite thingy that looks like a selector, btw.
    /*We also need an additional function to help with showing whether or not the current tile the mouse is over
     is ON TOP of a tile. A prime example of this being the player unit. We want the player unit to be highlighted and we want the cursor to disappear when you're hovering over them. So having a dictionary can help
    with that*/


    private void Awake()
    {
        UnityEngine.Debug.Log("Map Manager initialized");
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        generateGrid();
    }

    private void generateGrid()
    {
        UnityEngine.Debug.Log("Initializing Grid");
        var tileMap = gameObject.GetComponentInChildren<Tilemap>();
        map = new Dictionary<Vector2Int, HideAndShowScript>();
        BoundsInt bounds = tileMap.cellBounds;

        int idCounter = 1; // Counter for generating unique IDs

        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var tileLocation = new Vector3Int(x, y, z);
                    var tileKey = new Vector2Int(x, y);

                    if (tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
                    {
                        var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                        var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                        // Check if the tile is blocked by a defense structure
                        bool isBlockedByStructure = CheckIfBlockedByStructure(cellWorldPosition);
                        overlayTile.isBlocked = isBlockedByStructure;

                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 0.0001f);
                        //we plus the sortingOrder by 1 at the end to make the "overlayTiles" visible.
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder+1;

                        // Assign a unique ID to the tile
                        string tileID = "Tile_" + idCounter;
                        idCounter++;
                        overlayTile.gameObject.name = tileID;

                        overlayTile.gridLocation = tileLocation;
                        map.Add(tileKey, overlayTile);
                        }
                }
            }
        }

        if (GameEventSystem.current != null)
        {
            GameEventSystem.current.generatedGrid();
        }
    }

    //Copied and pasted the getNeighbourTiles function from AStarPathfinder to use in other scripts, seeing as it is logic related to the Map and not necessarily the pathfinding script.
    public List<HideAndShowScript> getNeighbourTiles(HideAndShowScript currentOverlayTile, List<HideAndShowScript> searchableTiles)
    {

        Dictionary<Vector2Int, HideAndShowScript> tileToSearch = new Dictionary<Vector2Int, HideAndShowScript>();

        if(searchableTiles.Count > 0)
        {
            foreach (var item in searchableTiles)
            {
                tileToSearch.Add(item.grid2DLocation, item);
            }
        } else
        {
            tileToSearch = map;
        }

        List<HideAndShowScript> neighbours = new List<HideAndShowScript>();

        //Top tile
        Vector2Int locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y + 1);

        if (tileToSearch.ContainsKey(locationToCheck))
        {
            if(Mathf.Abs(currentOverlayTile.gridLocation.z - tileToSearch[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(tileToSearch[locationToCheck]);
        }

        //Bottom tile
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y - 1);

        if (tileToSearch.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - tileToSearch[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(tileToSearch[locationToCheck]);
        }

        //Right tile
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x + 1, currentOverlayTile.gridLocation.y);

        if (tileToSearch.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - tileToSearch[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(tileToSearch[locationToCheck]);
        }

        //Left tile
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x - 1, currentOverlayTile.gridLocation.y);

        if (tileToSearch.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - tileToSearch[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(tileToSearch[locationToCheck]);
        }

        return neighbours;
    }

    //Gets the tile at position | can only be called AFTER the Map has been generated.
    public HideAndShowScript getTileAtPosition(int x, int y)
    {
        var tileKey = new Vector2Int(x, y);
        if (map.ContainsKey(tileKey))
        {
            return map[tileKey];
        }
        return null;
    }


    public List<KeyValuePair<Vector2Int, HideAndShowScript>> GetOverLayTiles()
    {
        List<KeyValuePair<Vector2Int, HideAndShowScript>> overlayTiles = new List<KeyValuePair<Vector2Int, HideAndShowScript>>();

        foreach (var kvp in map)
        {
            overlayTiles.Add(new KeyValuePair<Vector2Int, HideAndShowScript>(kvp.Key, kvp.Value));
        }

        return overlayTiles;
    }

    private bool CheckIfBlockedByStructure(Vector2 cellWorldPosition)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(cellWorldPosition);

        foreach (Collider2D collider in colliders)
        {
            DefenceStructure defenseStructure = collider.GetComponent<DefenceStructure>();
            if (defenseStructure != null)
            {
                // The tile is blocked by a defense structure
                return true;
            }
        }

        // The tile is not blocked by a defense structure
        return false;
    }

}
