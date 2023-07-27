using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PunchingBagMovement : MonoBehaviour
{
    private EnemyUnitScript enemyUnit;
    private AStarPathfinder pathfinder;
    private List<HideAndShowScript> path;

    public float speed;

    private RangefinderMovement rangeFinder;
    private List<HideAndShowScript> inRangeTiles = new List<HideAndShowScript>();

    private bool hasMoved;
    public bool turnOver;

    
    private PlayerUnitScript targetPlayer;

    void Start()
    {
        pathfinder = new AStarPathfinder();
        path = new List<HideAndShowScript>();
        rangeFinder = new RangefinderMovement();

        enemyUnit = GetComponent<EnemyUnitScript>();
        GameEventSystem.current.onPlayerStartTurn += refreshActions;
    }

    void Update()
    {
        //This update function constantly checks if the enemyUnit has a playerUnit in range to move to
        inRangeTiles = rangeFinder.GetTilesInRange(enemyUnit.activeTile, enemyUnit.movementRange);

        if (CombatStateManager.CSInstance.State == CombatState.EnemyTurn && turnOver == false)
        {
            if (!hasMoved)
            {
                FindPathToPlayer();
                //Debug.Log("Path value: " + path);
            }

            if (hasMoved && path.Count > 0)
            {
                MoveAlongPath();
            }
            else if (hasMoved == true && path.Count <= 0)
            {
                Debug.Log("Turn Over");
                turnOver = true;
                //CombatStateManager.CSInstance.UpdateCombatState(CombatState.PlayerTurn);
            }
        }
    }

    //This is used for debugging purposes, should be removed in the final build of the game
    public void TriggerEnemyMovement()
    {
        hasMoved = false;
    }

    private void FindPathToPlayer()
    {
        PlayerUnitScript player = FindObjectOfType<PlayerUnitScript>();

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

            Debug.Log("Path : " + path);
            hasMoved = true;
        }
    }

    private void MoveAlongPath()
    {
        if (!hasMoved)
        {
            return;
        }
        enemyUnit.activeTile.isBlocked = false;
        var step = speed * Time.deltaTime;
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

    private void refreshActions()
    {
        hasMoved = false;
        turnOver = false;
    }

    //This function adjusts the position of the character/unit on the gameplay tile
    private void PositionCharacterOnTile(HideAndShowScript tile)
    {
        enemyUnit.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z + 1);
        enemyUnit.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 2;
        enemyUnit.activeTile = tile;
        enemyUnit.activeTile.isBlocked = true;
        CombatStateManager.CSInstance.UpdateCombatState(CombatState.EnemyTurn);
    }
}