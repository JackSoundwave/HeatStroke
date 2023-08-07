using UnityEngine;

public class PlayerUnitScript : MonoBehaviour
{
    //Tile character is standing on
    public HideAndShowScript activeTile;

    //pretty sure I don't have to explain this
    public int health, movementRange, attackDmg, attackRange, maxHealth;

    //booleans to dictate whether or not the player has moved or has attacked already. canMove is set to true and hasAttacked is set to false.
    //If true, then the event manager tells the MouseController script that the unit can move after being selected.\
    //isAttacking is to determine the current pUnit state. If the unit is attacking, the selected tile afterwards gets an attack on it
    public bool canMove, hasAttacked, attackPrimed, isSelected, isAttacking, isMoving, isDeploying;


    public Material normal;
    public Material selected;

    private void Awake()
    {
        //when the onPlayerStartTurn action gets called, all actions are refreshed for this unit.
        GameEventSystem.current.onPlayerStartTurn += refreshActions;

        //So this may seem a bit counterintuitive, why would we want to deselect this unit if we attempt to select a different unit?
        //You would be right, it doesn't make sense
        //However, we want the player to only have ONE unit selected at any given time.
        //So, we invoke the unitselect action to trigger this de-select, just before selecting another unit to prevent conflicts from arising :)
        addSelfToList();
    }

    void OnDestroy()
    {
        //Unsub from all actions to clear up space and save on data.
        //idrk how it works but it's good practice apparently.
        GameEventSystem.current.onPlayerStartTurn -= refreshActions;
    }

    private void Update()
    {
        //just checking every frame to see if the health of this specific unit is 0, if so, kill the gameObject.
        if (health <= 0)
        {
            killSelf();
        }
    }
    
    private void killSelf()
    {
        //we'll add like, animations and stuff inside this script later
        activeTile.isBlocked = false;
        activeTile.entity = null;
        Destroy(gameObject);
    }

    private void refreshActions()
    {
        Debug.Log("Actions refreshed");
        canMove = true;
        hasAttacked = false;
        attackPrimed = false;
        isSelected = false;
        isAttacking = false;
    }

    private void addSelfToList()
    {
        for (int i = 0; i < GameEventSystem.current.playerUnits.Length; i++)
        {
            if (GameEventSystem.current.playerUnits[i] == null)
            {
                GameEventSystem.current.playerUnits[i] = gameObject;
                return;
            }
        }
        //This block executes if an element could not be found, needs proper error handling.
        Debug.Log("Could not add element to unitList in the GameEventSystem");
    }

    public void removeSelfFromList()
    {
        for(int i =0; i < GameEventSystem.current.playerUnits.Length; i++)
        {
            if (GameEventSystem.current.playerUnits[i] == gameObject)
            {
                GameEventSystem.current.playerUnits[i] = null;
                return;
            }
        }
    }

    private void onConfirmDeployPressed()
    {
        isDeploying = false;        
    }

    public HideAndShowScript getActiveTile()
    {
        return activeTile;
    }
}
