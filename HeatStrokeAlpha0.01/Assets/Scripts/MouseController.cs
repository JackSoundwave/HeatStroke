using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

//Adding this in for brevity and clarity's sake, this script is meant for the user to use during gameplay.
//Think of the gamepad, people call it a "Controller" because you use it to "control" what's happening on screen.
//Hence this class is called "MouseController"

public class MouseController : MonoBehaviour
{

    //Made MouseController public static because we'll be using it for literally everything in relation to controlling the game.
    public static MouseController ActiveInstance { get; private set; }

    //Adding a public deployableUnits array to neccesitate the deploying of said units.
    public GameObject[] deployableUnits = new GameObject[3];

    private void Awake()
    {
        ActiveInstance = this;
    }

    private void OnDestroy()
    {
        if (ActiveInstance == this)
        {
            ActiveInstance = null;
        }
    }
    public GameObject playerUnitPrefab;  
    private PlayerUnitScript pUnit;
    public EnemyUnitScript targetedEnemyUnit;
    public float speed;
    public GameObject cursor;
    private AStarPathfinder pathFinder;
    private RangefinderMovement rangeFinder;
    private List<HideAndShowScript> path;
    private List<HideAndShowScript> inRangeTiles = new List<HideAndShowScript>();
    private Camera mainCamera;

    //Start is called before the first frame update
    void Start()
    {
        pathFinder = new AStarPathfinder();
        path = new List<HideAndShowScript>();
        rangeFinder = new RangefinderMovement();
        inRangeTiles = new List<HideAndShowScript>();
        mainCamera = Camera.main;
    }

    //LateUpdate is called at the END of a previous update function call.
     void LateUpdate()
     {
        var focusedTileHit = GetFocusedOnTile();

        if (focusedTileHit.HasValue)
        {
            switch (focusedTileHit.Value.collider.gameObject.GetComponent<MonoBehaviour>())
            {
                //if raycast detects an EnemyUnitScript attached to a gameObject
                case EnemyUnitScript _:

                    //Debug.Log("Enemy unit detected!");
                    //sets the current targeted Enemy to whatever the player is currently selecting.
                    //positions the cursor on the Enemy's tile, it's a minor UI bug that gets fixed with this line
                    //transform.position = focusedTileHit.Value.collider.gameObject.GetComponent<EnemyUnitScript>().activeTile.transform.position;
                    targetedEnemyUnit = focusedTileHit.Value.collider.gameObject.GetComponent<EnemyUnitScript>();
                    //transform.position = targetedEnemyUnit.activeTile;
                    break;

                //if the raycast detects a playerScript attached to a gameObject
                case PlayerUnitScript _:
                    //Debug.Log("Player unit detected!");

                    //setting the targeted EnemyUnit to null when NOT hovering over it in the scene.
                    targetedEnemyUnit = null;
                    transform.position = focusedTileHit.Value.collider.gameObject.GetComponent<PlayerUnitScript>().activeTile.transform.position;

                    //can only be selected if the current state is player turn.
                    if (Input.GetMouseButtonDown(0) && CombatStateManager.CSInstance.State == CombatState.PlayerTurn)
                    {
                        GameEventSystem.current.unitSelected();
                        pUnit.isSelected = true;
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        pUnit.isSelected = false;
                    }
                    break;
                    
                    //if raycast detects an empty tile.
                 case HideAndShowScript hideAndShowScript:

                    transform.position = hideAndShowScript.transform.position;
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = hideAndShowScript.GetComponent<SpriteRenderer>().sortingOrder + 1;

                    //setting the targeted EnemyUnit to null when NOT hovering over it in the scene.
                    targetedEnemyUnit = null;

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (pUnit == null)
                        {
                            //spawn a unit if prefab 1 is null
                            pUnit = Instantiate(playerUnitPrefab).GetComponent<PlayerUnitScript>();
                            PositionCharacterOnTile(hideAndShowScript);
                        }
                        else
                        {
                            //path = pathFinder.FindPath(pUnit.activeTile, hideAndShowScript, inRangeTiles);
                        }
                    }
                    break;

                    //default case so that it still runs despite detecting something invalid (like something out of bounds, for example)
                default:
                    Debug.Log("Nothing detected");
                    break;
            }
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
    public void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        pUnit.activeTile.isBlocked = false;
        var zIndex = path[0].transform.position.z;
        pUnit.transform.position = Vector2.MoveTowards(pUnit.transform.position, path[0].transform.position, step);
        pUnit.transform.position = new Vector3(pUnit.transform.position.x, pUnit.transform.position.y, zIndex);

        if(Vector2.Distance(pUnit.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }
        if
        (path.Count == 0)
        {
            GetInRangeTiles();
            pUnit.isMoving = true;
        }
    }

    //Meant to work with the player movement scripts to get tiles in range
    public void GetInRangeTiles()
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
    public void ShowInRangetiles()
    {
        inRangeTiles = rangeFinder.GetTilesInRange(pUnit.activeTile, pUnit.movementRange);
        foreach (var item in inRangeTiles)
        {
            item.ShowTile();
        }
    }

    //Refer to previous comment.
    public void HideInRangeTiles()
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

    public List<HideAndShowScript> generatePath()
    {
        var targetTile = GetFocusedOnTile();
        if (targetTile.HasValue && targetTile.Value.collider.gameObject.GetComponent<HideAndShowScript>() == true)
        {
            HideAndShowScript endTile = targetTile.Value.collider.gameObject.GetComponent<HideAndShowScript>();
            path = pathFinder.FindPath(pUnit.activeTile, endTile, inRangeTiles);
            return path;
        }
        return new List<HideAndShowScript>();
    }

    public List<HideAndShowScript> getPath()
    {
        return path;
    }

    //This function adjusts the position of the character/unit on the gameplay tile
    private void PositionCharacterOnTile(HideAndShowScript tile)
    {
        pUnit.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z + 1);
        pUnit.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 2;
        pUnit.activeTile = tile;
        pUnit.activeTile.isBlocked = true;
    }


}
