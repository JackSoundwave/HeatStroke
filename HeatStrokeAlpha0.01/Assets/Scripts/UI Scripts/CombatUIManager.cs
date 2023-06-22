using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButtonScript : MonoBehaviour
{
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private TextMeshProUGUI _combatstateText;
    [SerializeField] private Button _primeAttackButton;

    // Start is called before the first frame update
    void Awake()
    {
        CombatStateManager.OnCombatStateChanged += CombatStateManagerOnOnCombatStateChanged;
    }

    private void OnDestroy()
    {
        
    }

    private void CombatStateManagerOnOnCombatStateChanged(CombatState state)
    {
        //These two lines basically say "if the current state is the player turn, then the button is interactable, otherwise, the player can't click those buttons if it's not their turn.
        _primeAttackButton.interactable = state == CombatState.PlayerTurn;
        _endTurnButton.interactable = state == CombatState.PlayerTurn;
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
