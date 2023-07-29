using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyAIStateManager : MonoBehaviour
{
    //==EnemyAI States==//
    EnemyAIBaseScript currentState;
    public EnemyUnitScript thisUnit;
    public EnemyAIidleState idleState = new EnemyAIidleState();
    public EnemyIsMovingState isMovingState = new EnemyIsMovingState();
    //==EnemyAI States==//

    //==RangeFinders and pathfinder==//
    //we're supposed to use these, but leaving them in the PunchingBagMovement script for now due to time constraints
    //public AStarPathfinder pathfinder = new AStarPathfinder();
    //public RangefinderMovement rangefinderMove = new RangefinderMovement();
    //==RangeFinders and pathfinder==//

    //==Enemy Movement stuff==//
    private List<HideAndShowScript> path;
    private List<HideAndShowScript> inRangeTiles = new List<HideAndShowScript>();
    public PunchingBagMovement movement;
    //==Enemy Movement stuff==//

    private void Awake()
    {
        movement = GetComponent<PunchingBagMovement>();
        thisUnit = GetComponent<EnemyUnitScript>();
    }
    void Start()
    {
        //Starting state for the enemy !!DO NOT CHANGE THIS LINE!!
        currentState = idleState;

        //Referencing "THIS" state.
        currentState.EnterState(this);

        GameEventSystem.current.onEnemyTurnStart += Test;
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    // Update is called once per frame
    public void SwitchState(EnemyAIBaseScript state)
    {
        currentState = state;
        state.EnterState(this);
    }

    private void Test()
    {
        SwitchState(isMovingState);
    }
    /*Attempting to move all the movement code from PunchingBagMovement into the stateManager for the enemyAI instead.*/
}
