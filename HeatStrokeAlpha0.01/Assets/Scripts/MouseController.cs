using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

//Adding this in for clarity's sake, this script is meant for the user to use during gameplay.
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
        SceneManager.sceneLoaded += deployUnitSetupOnSceneLoad;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= deployUnitSetupOnSceneLoad;
        GameEventSystem.current.onResetDeployPressed -= deployUnitSetup;
        if (ActiveInstance == this)
        {
            ActiveInstance = null;
        }
    }

    public GameObject playerUnitPrefab;
    //serializing these fields for debugging purposes
    [SerializeField]
    private PlayerUnitScript hoveredPlayerUnit;

    //This pUnit variable is the currently selected unit for the mouse.
    public PlayerUnitScript pUnit;

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
        GameEventSystem.current.onResetDeployPressed += deployUnitSetup;
    }

    //LateUpdate is called at the END of a previous update function call.
    void LateUpdate()
    {
        var focusedTileHit = GetFocusedOnTile();

        //==PLAYER TURN LOGIC==//
        if (CombatStateManager.CSInstance?.State == CombatState.PlayerTurn)
        {
            if (focusedTileHit.HasValue)
            {
                switch (focusedTileHit.Value.collider.gameObject.GetComponent<MonoBehaviour>())
                {
                    //if raycast detects an EnemyUnitScript attached to a gameObject
                    case EnemyUnitScript _:

                        //Debug.Log("Enemy unit detected!");
                        //sets the current targeted Enemy to whatever the player is currently selecting.
                        //positions the cursor on the Enemy's tile, it's a minor UI bug that gets fixed with this line

                        targetedEnemyUnit = focusedTileHit.Value.collider.gameObject.GetComponent<EnemyUnitScript>();
                        transform.position = focusedTileHit.Value.collider.gameObject.GetComponent<EnemyUnitScript>().activeTile.transform.position;
                        hoveredPlayerUnit = null;

                        //transform.position = targetedEnemyUnit.activeTile;
                        break;

                    //if the raycast detects a playerScript attached to a gameObject
                    case PlayerUnitScript _:
                        //Debug.Log("Player unit detected!");

                        //setting the targeted EnemyUnit to null when NOT hovering over it in the scene.
                        targetedEnemyUnit = null;
                        hoveredPlayerUnit = focusedTileHit.Value.collider.gameObject.GetComponent<PlayerUnitScript>();
                        transform.position = focusedTileHit.Value.collider.gameObject.GetComponent<PlayerUnitScript>().activeTile.transform.position;

                        //can only be selected if the current state is player turn.
                        if (Input.GetMouseButtonDown(0) && CombatStateManager.CSInstance.State == CombatState.PlayerTurn)
                        {
                            GameEventSystem.current.unitSelected();
                            pUnit = hoveredPlayerUnit;
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


                        if (Input.GetMouseButtonDown(0))
                        {
                            if (pUnit == null)
                            {
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
        //==PLAYER TURN LOGIC==//



        //==DEPLOY PHASE LOGIC==//
        else if (CombatStateManager.CSInstance?.State == CombatState.DeployPhase)
        {
            if (focusedTileHit.HasValue)
            {
                switch (focusedTileHit.Value.collider.gameObject.GetComponent<MonoBehaviour>())
                {
                    //if raycast detects an empty tile.
                    case HideAndShowScript hideAndShowScript:

                        transform.position = hideAndShowScript.transform.position;
                        gameObject.GetComponent<SpriteRenderer>().sortingOrder = hideAndShowScript.GetComponent<SpriteRenderer>().sortingOrder + 1;


                        if (Input.GetMouseButtonDown(0))
                        {
                            //checks if all units in the array are null or not
                            if (!deployableUnits.All(GameObject => GameObject == null) && hideAndShowScript.isBlocked == false)
                            {
                                if(hideAndShowScript.isDeployTile == true) 
                                {
                                    instantiateUnitAtPosition(hideAndShowScript);
                                    GameEventSystem.current.deployUnit(); //call the deployUnit method
                                }
                                else
                                {
                                    //play error sound
                                    Debug.Log("No, stop it. That's enough. Why are you like this.");
                                }
                            }
                            else
                            {
                                //play error sound here
                                Debug.Log("No");
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
        else
        {

        }
        //==DEPLOY PHASE LOGIC==//
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

    //==Path & Movement Related==//
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
        if (pUnit != null)
        {
            inRangeTiles = rangeFinder.GetTilesInRange(pUnit.activeTile, pUnit.movementRange);
            foreach (var item in inRangeTiles)
            {
                item.HideTile();
            }
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
            if (item.isBlocked)
            {
                item.DyeTileBlue();
            }
            else
            {
                item.ShowTile();
            }
        }
    }

    //Moves a unit along a path
    public void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        pUnit.activeTile.isBlocked = false;
        var zIndex = path[0].transform.position.z;
        pUnit.transform.position = Vector2.MoveTowards(pUnit.transform.position, path[0].transform.position, step);
        pUnit.transform.position = new Vector3(pUnit.transform.position.x, pUnit.transform.position.y, zIndex);

        if (Vector2.Distance(pUnit.transform.position, path[0].transform.position) < 0.0001f)
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
    //==Path & Movement Related==//


    //==Selection Related==//
    private PlayerUnitScript tileCheckerPlayerUnit(HideAndShowScript tileToCheck)
    {
        for(int i = 0; i < GameEventSystem.current.playerUnits.Length; i++)
        {
            if (GameEventSystem.current.playerUnits[i].GetComponent<PlayerUnitScript>().activeTile == tileToCheck)
            {
                return GameEventSystem.current.playerUnits[i].GetComponent<PlayerUnitScript>();
            }
        }
        //returns null if the value can't be found
        return null;
    }

    private EnemyUnitScript tileCheckerEnemyUnit(HideAndShowScript tileToCheck)
    {
        foreach(EnemyUnitScript enemyUnit in GameEventSystem.current.enemyUnits)
        {
            if(enemyUnit.activeTile == tileToCheck)
            {
                return enemyUnit;
            } 
        }
        //returns null if no value can be found.
        return null;
    }
    //==Selection Related==/

    //==Deployment Phase Related==//
    private void deployUnitSetup()
    {
        if (deployableUnits.Length == GameEventSystem.current?.unitsToDeploy.Length)
        {
            for (int i = 0; i < deployableUnits.Length; i++)
            {
                deployableUnits[i] = GameEventSystem.current?.unitsToDeploy[i];
            }
        }
    }
    private void deployUnitSetupOnSceneLoad(Scene currentScene, LoadSceneMode mode)
    {
        if (deployableUnits.Length == GameEventSystem.current?.unitsToDeploy.Length)
        {
            for (int i = 0; i < deployableUnits.Length; i++)
            {
                deployableUnits[i] = GameEventSystem.current?.unitsToDeploy[i];
            }
        }
    }

    //Passes the unit from the deployableUnits array (that is not null) then instantiates it.
    public void instantiateUnitAtPosition(HideAndShowScript tileToSpawnAt)
    {
        for(int i = 0; i < deployableUnits.Length; i++)
        {
            if (deployableUnits[i] != null) 
            {
                GameObject newPlayerUnitGO = Instantiate(deployableUnits[i]);
                PlayerUnitScript newPlayerUnit = newPlayerUnitGO.GetComponent<PlayerUnitScript>();
                pUnit = newPlayerUnit;
                PositionCharacterOnTile(tileToSpawnAt);
                deployableUnits[i] = null;
                pUnit = newPlayerUnit;
                break;
            }
        }
    }
    //==Deployment Phase Related==//

}
