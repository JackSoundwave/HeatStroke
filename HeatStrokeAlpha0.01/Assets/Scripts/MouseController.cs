using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//Adding this in for brevity and clarity's sake, this script is meant for the user to use during gameplay.
//Think of the gamepad, people call it a "Controller" because you use it to "control" what's happening on screen.
//Hence this class is called "MouseController"

public class MouseController : MonoBehaviour
{
    public GameObject playerUnitPrefab;
    private PlayerUnitScript pUnit;

    public float speed;
    public GameObject cursor;

    private AStarPathfinder pathFinder;
    private RangefinderMovement rangeFinder;

    private List<HideAndShowScript> path;
    private List<HideAndShowScript> inRangeTiles = new List<HideAndShowScript>();
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        pathFinder = new AStarPathfinder();
        path = new List<HideAndShowScript>();
        rangeFinder = new RangefinderMovement();
        inRangeTiles = new List<HideAndShowScript>();
        mainCamera = Camera.main;
    }

    // LateUpdate is called at the END of a previous update function call.
     void LateUpdate()
     {
        var focusedTileHit = GetFocusedOnTile();
        Debug.Log("FocusedTileHit value: " + focusedTileHit);

        if (focusedTileHit.HasValue)
        {
            switch (focusedTileHit.Value.collider.gameObject.GetComponent<MonoBehaviour>())
            {
                case HideAndShowScript hideAndShowScript:
                    transform.position = hideAndShowScript.transform.position;
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = hideAndShowScript.GetComponent<SpriteRenderer>().sortingOrder + 1;

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (pUnit == null)
                        {
                            pUnit = Instantiate(playerUnitPrefab).GetComponent<PlayerUnitScript>();
                            PositionCharacterOnTile(hideAndShowScript);
                            GetInRangeTiles();
                        }
                        else
                        {
                            path = pathFinder.FindPath(pUnit.activeTile, hideAndShowScript, inRangeTiles);
                        }
                    }
                    break;

                    //if raycast detects a playerScript attached to a gameObject
                case PlayerUnitScript _:
                    Debug.Log("Player unit detected!");
                    if (Input.GetMouseButtonDown(0))
                    {
                        pUnit.isSelected = true;
                    }
                    break;

                    //if raycast detects an EnemyUnitScript attached to a gameObject
                case EnemyUnitScript _:
                    Debug.Log("Enemy unit detected!");
                    break;

                    //default case so that it still runs despite detecting something invalid (like something out of bounds)
                default:
                    Debug.Log("Nothing detected");
                    break;
            }
        }

        if (path.Count > 0)
         {
             MoveAlongPath();
         }
     }

   /* void LateUpdate()
    {
        var focusedTileHit = GetFocusedOnTile();

        if (focusedTileHit.HasValue)
        {
            HideAndShowScript overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<HideAndShowScript>();
            transform.position = overlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder+1;

            if (Input.GetMouse)
        }
    }*/

    //Moves an entity along the path setup by the A* pathfinding script, also utilizes the MapManager getInRangeTiles() function to retrieve the positions of tiles near the Unit.
    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        
        var zIndex = path[0].transform.position.z;
        pUnit.transform.position = Vector2.MoveTowards(pUnit.transform.position, path[0].transform.position, step);
        pUnit.transform.position = new Vector3(pUnit.transform.position.x, pUnit.transform.position.y, zIndex);

        if(Vector2.Distance(pUnit.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            GetInRangeTiles();
        }
    }

    //Meant to work with the player movement scripts to get tiles in range
    private void GetInRangeTiles()
    {
        foreach (var item in inRangeTiles) 
        {
            item.HideTile();
        }

        inRangeTiles = rangeFinder.GetTilesInRange(pUnit.activeTile, pUnit.movementRange);

        foreach (var item in inRangeTiles)
        {
            item.ShowTile();
        }
    }

    //Not used yet, but will be used soon for the state machine of the player.
    private void ShowInRangetiles()
    {
        inRangeTiles = rangeFinder.GetTilesInRange(pUnit.activeTile, pUnit.movementRange);
        foreach (var item in inRangeTiles)
        {
            item.HideTile();
        }
    }

    //Refer to previous comment.
    private void HideInRangeTiles()
    {
        inRangeTiles = rangeFinder.GetTilesInRange(pUnit.activeTile, pUnit.movementRange);
        foreach (var item in inRangeTiles)
        {
            item.HideTile();
        }
    }

    public RaycastHit2D? GetFocusedOnTile()
    {

        //To tell you what's happening here, essentially we're getting the position of the mouse relative to 3D coordinates, by casting a ray.
        //Although, since we want the position of the mouse in 2D terms, we switch it from Vector3, which is 3D, to Vector2, which is 2d.
        //we also use raycast to actually "select" the tile to focus on it as well.
        //This runs every frame and acts as a "mouseover" function for the cursor. B)

        //Raycasting is extremely similar to how "hitscan" works in video games. It's legitimately the same principle, but instead of using the aiming reticle, we're using the mouse :)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        if(hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }
        return null;
    }

    //This function adjusts the position of the character/unit on the gameplay tile
    private void PositionCharacterOnTile(HideAndShowScript tile)
    {
        pUnit.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z + 1);
        pUnit.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
        pUnit.activeTile = tile;
    }
}
