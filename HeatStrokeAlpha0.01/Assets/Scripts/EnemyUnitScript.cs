using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitScript : MonoBehaviour
{
    //Tile character is standing on
    public HideAndShowScript activeTile;

    //pretty sure I don't have to explain this
    public int health, movementRange, attackDmg, attackRange, maxHealth, speed;


    public GameObject upAttackVFX, downAttackVFX, rightAttackVFX, leftAttackVFX;

    [SerializeField]
    private GameObject currentVFX;

    //booleans to dictate whether or not the player has moved or has attacked already. canMove is set to true and hasAttacked is set to false.
    //If true, then the event manager tells the MouseController script that the unit can move after being selected.
    //isAttacking is to determine the current pUnit state. If the unit is attacking, the selected tile afterwards gets an attack on it

    //Note: when an enemy first spawns, it's default state will always be "isIdle", it will then move to the player, and prime an attack against it if it detects a targetable entity.
    public bool hasMoved, hasAttacked, isAttacking, attackPrimed, isMoving, turnOver;
    public bool isSelected;
    public HealthBar healthBar;

    private void Awake()
    {
        hideHealthBar();
        GameEventSystem.current.enemyUnits.Add(gameObject);
    }

    private void Start()
    {
        //upon starting, adds THIS unit to the EnemyUnit list in the GameEventSystem.
        healthBar.SetMaxHealth(maxHealth);
        GameEventSystem.current.onEnemyTurnEnd += refreshActions;
    }

    private void OnDestroy()
    {
        GameEventSystem.current.onEnemyTurnEnd -= refreshActions;
    }

    private void Update()
    {
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            activeTile.isBlocked = false;
            GameEventSystem.current.enemyUnitsToRemove.Add(gameObject);
            Death();
        }

        if(MouseController.ActiveInstance?.targetedEnemyUnit == this)
        {
            showHealthBar();
            HighlightAttackVFX();
        }
        else
        {
            hideHealthBar();
            UnHighlightAttackVFX();
        }
    }

    private void Death()
    {
        GameEventSystem.current.enemyDeath();
        activeTile.isBlocked = false;
        if(currentVFX != null)
        {
            Destroy(currentVFX);
        }
        Destroy(this.gameObject);
    }

    public void Attack(HideAndShowScript targetTile)
    {
        //attack logic is supposed to go here
        Debug.Log("Attacking Tile" + targetTile.name);

        //resetting attack state for future turns
        isAttacking = false;
        hasAttacked = true;
    }

    private void showHealthBar()
    {
        healthBar.gameObject.SetActive(true);
    }

    private void hideHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }

    private void refreshActions()
    {
        hasAttacked = false;
        hasMoved = false;
        turnOver = false;
    }

    public void enemyUnitFadeOut()
    {

    }

    public void SpawnUpAttackVFX(HideAndShowScript tileToSpawn)
    {
        Destroy(currentVFX);
        currentVFX = Instantiate(upAttackVFX);
        currentVFX.transform.position = tileToSpawn.transform.position;
    }

    public void SpawnDownAttackVFX(HideAndShowScript tileToSpawn)
    {
        Destroy(currentVFX);
        currentVFX = Instantiate(downAttackVFX);
        currentVFX.transform.position = tileToSpawn.transform.position;
    }

    public void SpawnRightAttackVFX(HideAndShowScript tileToSpawn)
    {
        Destroy(currentVFX);
        currentVFX = Instantiate(rightAttackVFX);
        currentVFX.transform.position = tileToSpawn.transform.position;
    }

    public void SpawnLeftAttackVFX(HideAndShowScript tileToSpawn)
    {
        Destroy(currentVFX);
        currentVFX = Instantiate(leftAttackVFX);
        currentVFX.transform.position = tileToSpawn.transform.position;
    }

    public void DestroyCurrentVFX()
    {
        Destroy(currentVFX.gameObject);
    }

    private void HighlightAttackVFX()
    {
        if(currentVFX != null)
        {
            currentVFX.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private void UnHighlightAttackVFX()
    {
        if(currentVFX != null)
        {
            currentVFX.GetComponent<SpriteRenderer>().color = new Color (1, 1, 1, 1);
        }
    }
}
