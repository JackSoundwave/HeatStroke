using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PunchingBagMovement : MonoBehaviour
{
    private EnemyUnitScript enemyUnit;
    private AStarPathfinder pathfinder;
    public List<HideAndShowScript> path;
    public RangefinderMovement rangeFinder;

    //public float speed = 3;
    public List<HideAndShowScript> inRangeTiles;

    //Target list so that the unit can select the most optimal target.
    private List<PlayerUnitScript> targetPlayers = new List<PlayerUnitScript>();
    private List<DefenceStructure> targetStructures = new List<DefenceStructure>();


    private PlayerUnitScript targetPlayer;
    private DefenceStructure targetStructure;

    private void Awake()
    {
        enemyUnit = GetComponent<EnemyUnitScript>();
        pathfinder = new AStarPathfinder();
        rangeFinder = new RangefinderMovement();
    }



    //Commented out this update function as the EnemyAI now incorporates the StateMachine design pattern for update handling.
    //This is to make sure that all the crazy amount of if() statements are kept relative to whatever state the enemy is currently in.
    //I'm not sure why I'm typing this as I already know that myself, but if anyone else bothers to read it, here you are! :)

    /*
    void Update()
    {
        //This update function constantly checks if the enemyUnit has a playerUnit in range to move to
        inRangeTiles = rangeFinder.GetTilesInRange(enemyUnit.activeTile, enemyUnit.movementRange);

        if (CombatStateManager.CSInstance.State == CombatState.EnemyTurn && enemyUnit.turnOver == false)
        {
            if (!enemyUnit.isMoving)
            {
                FindPathToPlayer();
                //Debug.Log("Path value: " + path);
            }

            if (enemyUnit.isMoving && path.Count > 0)
            {
                MoveAlongPath();
            }
            else if (enemyUnit.isMoving == true && path.Count <= 0)
            {
                Debug.Log("Turn Over");
                enemyUnit.turnOver = true;
                enemyUnit.isMoving = false;
                //CombatStateManager.CSInstance.UpdateCombatState(CombatState.PlayerTurn);
            }
        }
    }
    */

    //This is used for debugging purposes, should be removed in the final build of the game
    public void TriggerEnemyMovement()
    {
        enemyUnit.isMoving = false;
    }

    private void FindPlayersInMovementRange()
    {
        //clear the targetList to instead find players within movement range
        targetPlayers.Clear();
        inRangeTiles = rangeFinder.GetTilesInRange(enemyUnit.activeTile, enemyUnit.movementRange);

        foreach(HideAndShowScript tile in inRangeTiles)
        {
            if(tile.entity.GetComponent<PlayerUnitScript>() != null)
            {
                PlayerUnitScript temp = tile.entity.GetComponent<PlayerUnitScript>();
                targetPlayers.Add(temp);
            }
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void ShuffleTargetPlayersList()
    {
        ShuffleList(targetPlayers);
    }

    public void ShuffleTargetStructuresList()
    {
        ShuffleList(targetStructures);
    }

    //GetAllPlayers is simply used when no players can be found within the movement range of the enemyUnit.
    public void getAllPlayers()
    {
        if(targetPlayers != null)
        {
            targetPlayers.Clear();
        }
        PlayerUnitScript[] temparray = FindObjectsOfType<PlayerUnitScript>();

        foreach(PlayerUnitScript target in temparray)
        {
            if(target != null) 
            {
                targetPlayers.Add(target);
            }
        }
    }

    //Same thing as GetAllPlayers, but for 
    public void getAllStructures()
    {
        if (targetStructures != null)
        {
            targetStructures.Clear();
        }
        DefenceStructure[] temparray = FindObjectsOfType<DefenceStructure>();

        foreach(DefenceStructure target in temparray)
        {
            if(target != null)
            {
                targetStructures.Add(target);
            }
        }
    }

    //this block executes if the pathValue after trying to resolve a path within movement range is equal to 0. Meaning, that no path was resolved because no target could be found within movement range.
    public void FindPathToRandomPlayer()
    {
        getAllPlayers();

        if(targetPlayers != null)
        {
            //Shuffle list before attempting to find path
            ShuffleTargetPlayersList();

            //Assign target 
            targetPlayer = targetPlayers[0];
            FindPathToPlayer();
        }
    }

    public void FindPathToRandomStructure()
    {
        getAllStructures();

        if (targetStructures != null)
        {
            //Shuffle list before attempting to find path
            ShuffleTargetStructuresList();

            //Assign target 
            targetPlayer = targetPlayers[0];
            FindPathToStructure();
        }
    }

    public void FindPathToPlayer()
    {
        PlayerUnitScript player = targetPlayer;

        if (player != null)
        {
            path.Clear(); //This removes the previous path


            List<HideAndShowScript> inRangeTiles = rangeFinder.GetTilesInRange(enemyUnit.activeTile, enemyUnit.movementRange);

            //finds path to the player if it's within it's movement range
            path = pathfinder.FindPath(enemyUnit.activeTile, player.activeTile, inRangeTiles);

            //this block executes if it cannot detect a player in it's movement range
            if (path.Count == 0)
            {

                List<HideAndShowScript> closestPath = FindClosestAdjacentTilePath(inRangeTiles, player.activeTile);

                if (closestPath.Count > 0)
                {

                    path = pathfinder.FindPath(enemyUnit.activeTile, closestPath[0], inRangeTiles);
                }
                else
                {
                    
                }
            }

            //Debug.Log("Path : " + path);
            //enemyUnit.isMoving = true;
        }
    }

    public void FindPathToStructure()
    {
        //yes I know I'm basically just copy pasting the target code leave me alone
        DefenceStructure player = targetStructure;

        if (player != null)
        {
            path.Clear(); //This removes the previous path


            List<HideAndShowScript> inRangeTiles = rangeFinder.GetTilesInRange(enemyUnit.activeTile, enemyUnit.movementRange);

            //finds path to the player if it's within it's movement range
            path = pathfinder.FindPath(enemyUnit.activeTile, player.activeTile, inRangeTiles);

            //this block executes if it cannot detect a player in it's movement range
            if (path.Count == 0)
            {

                List<HideAndShowScript> closestPath = FindClosestAdjacentTilePath(inRangeTiles, player.activeTile);

                if (closestPath.Count > 0)
                {

                    path = pathfinder.FindPath(enemyUnit.activeTile, closestPath[0], inRangeTiles);
                }
                else
                {

                }
            }

            //Debug.Log("Path : " + path);
            //enemyUnit.isMoving = true;
        }
    }

    public void MoveAlongPath()
    {
        Debug.Log("Moving along path :)");
        if (!enemyUnit.isMoving)
        {
            return;
        }
        enemyUnit.activeTile.isBlocked = false;
        enemyUnit.activeTile.entity = null;
        var step = enemyUnit.speed * Time.deltaTime;
        var zIndex = path[0].transform.position.z;
        enemyUnit.transform.position = Vector2.MoveTowards(enemyUnit.transform.position, path[0].transform.position, step);
        enemyUnit.transform.position = new Vector3(enemyUnit.transform.position.x, enemyUnit.transform.position.y, zIndex);
        if (Vector2.Distance(enemyUnit.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            GetInRangeTiles();
        }
    }

    private List<HideAndShowScript> FindClosestAdjacentTilePath(List<HideAndShowScript> tiles, HideAndShowScript targetTile)
    {
        int closestDistance = int.MaxValue;
        List<HideAndShowScript> closestPath = new List<HideAndShowScript>();

        //Find the closest tile in the list to the player tile
        foreach (var tile in tiles)
        {
            int distance = pathfinder.GetManhattanDistance(tile, targetTile);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPath.Clear();
                closestPath.Add(tile);
            }
            else if (distance == closestDistance)
            {
                closestPath.Add(tile);
            }
        }

        return closestPath;
    }

    private void GetInRangeTiles()
    {
        //This line below shows the in range tiles for movement of the Enemy Unit, causes issues UI wise, with the player movement, so I disabled it for now

        /*foreach (var item in inRangeTiles)
        {
            item.HideTile();
        }*/

        inRangeTiles = rangeFinder.GetTilesInRange(enemyUnit.activeTile, enemyUnit.movementRange);

        //This line below shows the in range tiles for movement of the Enemy Unit, causes issues UI wise, with the player movement, so I disabled it for now

        /*foreach (var item in inRangeTiles)
        {
           item.ShowTile();
        }*/
    }

    //This function adjusts the position of the character/unit on the gameplay tile
    private void PositionCharacterOnTile(HideAndShowScript tile)
    {
        enemyUnit.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z + 1);
        enemyUnit.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 2;
        enemyUnit.activeTile = tile;
        enemyUnit.activeTile.isBlocked = true;
        enemyUnit.activeTile.entity = enemyUnit.gameObject;
        //CombatStateManager.CSInstance.UpdateCombatState(CombatState.EnemyTurn);
    }
}
