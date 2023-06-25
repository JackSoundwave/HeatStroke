using UnityEngine;

/*This abstract class serves as the base for our playerStateManager. Keep in mind that different attacks can be pushed into different states.*/
public abstract class PlayerUnitBaseState
{
    //Enter state is the function that executes when it's recently changed.
   public abstract void EnterState(PlayerStateManager player);

    //Update state is the function that executes when changing to a new state.
   public abstract void UpdateState(PlayerStateManager player);
}
    
