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
    public EnemyCalculateTargetState calculate = new EnemyCalculateTargetState();
    public EnemyAttackPrimedState primingAttack = new EnemyAttackPrimedState();
    //==EnemyAI States==//

    //==RangeFinders and pathfinder==//
    //we're supposed to use these, but leaving them in the PunchingBagMovement script for now due to time constraints
    //public AStarPathfinder pathfinder = new AStarPathfinder();
    //public RangefinderMovement rangefinderMove = new RangefinderMovement();
    public AttackRangeFinder attackRangeFinder = new AttackRangeFinder();
    //==RangeFinders and pathfinder==//

    //==Enemy Movement stuff==//
    private List<HideAndShowScript> path;
    private List<HideAndShowScript> inRangeTiles = new List<HideAndShowScript>();
    public PunchingBagMovement movement;
    public EnemyAttack attack = new EnemyAttack();
    //==Enemy Movement stuff==//

    //==Calculation Variables==//
    public float playerBias, structureBias;

    [SerializeField]
    [Range(0, 5)]
    private int randomMin;

    [SerializeField]
    [Range(0, 5)]
    private int randomMax;
    //public bool rangedUnit;

    private void Awake()
    {
        movement = GetComponent<PunchingBagMovement>();
        thisUnit = GetComponent<EnemyUnitScript>();
        attack.setThisUnit(thisUnit);
        attack.attackRangeFinder = attackRangeFinder;
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
        SwitchState(calculate);
    }
    
    public int getRandomMin()
    {
        return randomMin;
    }

    public int getRandomMax()
    {
        return randomMax;
    }
    /*Attempting to move all the movement code from PunchingBagMovement into the stateManager for the enemyAI instead.*/
}
