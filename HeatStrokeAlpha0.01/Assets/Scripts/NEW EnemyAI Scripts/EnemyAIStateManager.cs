using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyAIStateManager : MonoBehaviour
{
    EnemyAIBaseScript currentState;
    public EnemyUnitScript thisUnit;
    public EnemyAIidleState idleState = new EnemyAIidleState();
    public EnemyIsMovingState isMovingState = new EnemyIsMovingState();


    public AStarPathfinder pathfinder = new AStarPathfinder();
    public RangefinderMovement rangefinderMove = new RangefinderMovement();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*Attempting to move all the movement code from PunchingBagMovement into the stateManager for the enemyAI instead.
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
    */
}
