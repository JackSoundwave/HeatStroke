using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DefensePunchingBag : MonoBehaviour
{
    private EnemyUnitScript enemyUnit;
    private AStarPathfinder pathfinder;
    private List<HideAndShowScript> path;

    public float speed;

    private RangefinderMovement rangeFinder;
    private List<HideAndShowScript> inRangeTiles = new List<HideAndShowScript>();

    private bool hasMoved;
    public bool turnOver;

    // Start is called before the first frame update
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
            if (!hasMoved && Communicator.Instance.AttackingDefense)
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
        DefenceStructure player = FindObjectOfType<DefenceStructure>();
        //Debug.Log("Player value: " + player);

        if (player != null)
        {
            path.Clear(); //This removes the previous path
            //Debug.Log("Generating Enemy path value");

            pathfinder.FindPath(enemyUnit.activeTile, player.activeTile, inRangeTiles);
            //This grabs the neighbourtiles of the player unit, using the instance2 variable declared in the MapManager script
            List<HideAndShowScript> playerNeighbourtiles = MapManager.instance2.getNeighbourTiles(player.activeTile, inRangeTiles);

            //This starts to filter any blocked tiles, currently isBlocked isn't being used so, just keep that in mind.
            playerNeighbourtiles.RemoveAll(tile => tile.isBlocked);

            if (playerNeighbourtiles.Count > 0)
            {
                //This chooses one of the random tiles near the playerneighbourtiles list to move to. Pretty sure this causes the enemy unit to constantly circle around the player in an erratic way
                HideAndShowScript destinationTile = playerNeighbourtiles[Random.Range(0, playerNeighbourtiles.Count)];

                // Find the path to the chosen destination tile
                path = pathfinder.FindPath(enemyUnit.activeTile, destinationTile, inRangeTiles);

                Debug.Log("Path : " + path);
                hasMoved = true;
            }
            else
            {
                //Debug.LogWarning("No valid path to player neighbourtiles");
            }
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