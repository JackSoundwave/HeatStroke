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

    
    private PlayerUnitScript targetPlayer;

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

    private void RandomlySelectPlayer()
    {
        
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
                    //requires additional handling here.
                }
            }

            //Debug.Log("Path : " + path);
            //enemyUnit.isMoving = true;
        }
    }

    public void MoveAlongPath()
    {
        Debug.Log("Moving along path");
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

    private List<HideAndShowScript> FindClosestAdjacentTilePath(List<HideAndShowScript> tiles, HideAndShowScript playerTile)
    {
        int closestDistance = int.MaxValue;
        List<HideAndShowScript> closestPath = new List<HideAndShowScript>();

        // Find the closest tile in the list to the player tile
        foreach (var tile in tiles)
        {
            int distance = pathfinder.GetManhattanDistance(tile, playerTile);
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

        inRangeTiles = rangeFinder.GetTilesInRange(enemyUnit.activeTile, 3);

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
