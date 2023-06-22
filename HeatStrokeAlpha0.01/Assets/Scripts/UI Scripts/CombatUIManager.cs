using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private TextMeshProUGUI combatStateText;
    [SerializeField] private Button _primeAttackButton;

    private CombatState currentCombatState;

    void Awake()
    {
        CombatStateManager.OnCombatStateChanged += CombatStateManagerOnOnCombatStateChanged;
        CombatStateManager.OnCombatStateChanged += UpdateCombatStateText;

        UpdateCombatStateText(CombatStateManager.CSInstance.State);
    }

    void OnDestroy()
    {
        CombatStateManager.OnCombatStateChanged -= CombatStateManagerOnOnCombatStateChanged;
    }

    private void UpdateCombatStateText(CombatState newState)
    {
        combatStateText.text = GetCombatStateText(newState);
    }

    private string GetCombatStateText(CombatState state)
    {
        switch (state)
        {
            case CombatState.DeployPhase:
                return "Deploy Phase";
            case CombatState.PlayerTurn:
                return "Player's Turn";
            case CombatState.EnemyTurn:
                return "Enemy's Turn";
            case CombatState.Victory:
                return "Victory!";
            case CombatState.Lose:
                return "Defeated";
            default:
                return "Unknown State";
        }
    }

    private void CombatStateManagerOnOnCombatStateChanged(CombatState state)
    {
        //These two lines basically say "if the current state is the player turn, then the button is interactable, otherwise, the player can't click those buttons if it's not their turn.
        _primeAttackButton.interactable = state == CombatState.PlayerTurn;
        _endTurnButton.interactable = state == CombatState.PlayerTurn;
    }

    public void EndTurn()
    {
        CombatStateManager.CSInstance.UpdateCombatState(CombatState.EnemyTurn);
    }
    
    public void PrimeAttackButtonPressed()
    {
        //code for switching the playerUnit state to "attackPrimed" goes here.
    }

    public void AttackExecuted()
    {
        //code for executing attack durng "primeAttack" state goes here
        //side note for myself: make sure to add "isAttacking" state for player. Or just make the attack instant and set the "hasAttacked" boolean to true after.
    }
}
