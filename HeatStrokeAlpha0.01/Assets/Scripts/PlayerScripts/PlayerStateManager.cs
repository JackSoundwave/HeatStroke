using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerUnitBaseState currentState;
    public PlayerUnitScript thisUnit;
    public PlayerUnitIdleState idleState = new PlayerUnitIdleState();
    public PlayerUnitSelectedState selectedState = new PlayerUnitSelectedState();
    public PlayerUnitAttackPrimedState attackPrimedState = new PlayerUnitAttackPrimedState();
    public PlayerAttack playerAttack = new PlayerAttack();
    public PlayerIsMovingState isMovingState = new PlayerIsMovingState();
    public MouseController cursor;
    public AttackRangeFinder attackRangeFinder = new AttackRangeFinder();

    private void Awake()
    {
        //This receives the currently activeInstance of the MouseController, which helps with all our other functions, and transfers the movement code, from the MouseController to our StateManager.
        //It's basically a reference alright, it's not that complicated
        //I also know it's kinda spaghetti
        //But you don't read these comments, let's be honest.

        cursor = MouseController.ActiveInstance;
        playerAttack.pUnit = thisUnit;
        playerAttack.attackRangeFinder = attackRangeFinder;
    }

    void Start()
    {
        //Starting state for our player !!DO NOT CHANGE THIS LINE!!
        currentState = idleState;

        //Referencing "THIS" state.
        currentState.EnterState(this);

        GameEventSystem.current.onUnitSelected += deselectSelf;
    }

    private void OnDestroy()
    {
        GameEventSystem.current.onUnitSelected -= deselectSelf;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(PlayerUnitBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    private void deselectSelf()
    {
        thisUnit.isSelected = false;
        thisUnit.attackPrimed = false;
        cursor.HideInRangeTiles();
    }
}
