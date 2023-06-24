using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerUnitBaseState currentState;
    public PlayerUnitScript thisUnit;
    public PlayerUnitIdleState idleState = new PlayerUnitIdleState();
    public PlayerUnitSelectedState selectedState = new PlayerUnitSelectedState();
    public PlayerUnitAttackPrimedState attackPrimedState = new PlayerUnitAttackPrimedState();
    public PlayerAttackMelee attackMelee = new PlayerAttackMelee();
    


    // Start is called before the first frame update
    void Start()
    {
        //Starting state for our player
        currentState = idleState;

        //Referencing "THIS" state.
        currentState.EnterState(this);
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

    
}
