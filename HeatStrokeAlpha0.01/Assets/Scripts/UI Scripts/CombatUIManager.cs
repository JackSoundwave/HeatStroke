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
    [SerializeField] private Button _deployConfirmButton;
    [SerializeField] private Button _deployResetButton;
    [SerializeField] private TextMeshProUGUI _unitsLeftText;
    [SerializeField] private TextMeshProUGUI _unitsLeftNumber;
    [SerializeField] private TextMeshProUGUI _enemyUnitsRemainingNumber;


    void Awake()
    {
        CombatStateManager.OnCombatStateChanged += CombatStateManagerOnOnCombatStateChanged;
        CombatStateManager.OnCombatStateChanged += UpdateCombatStateText;
    }

    private void Start()
    {
        UpdateCombatStateText(CombatStateManager.CSInstance.State);
    }
    void OnDestroy()
    {
        CombatStateManager.OnCombatStateChanged -= CombatStateManagerOnOnCombatStateChanged;
        CombatStateManager.OnCombatStateChanged -= UpdateCombatStateText;
    }

    private void UpdateCombatStateText(CombatState newState)
    {
        combatStateText.text = GetCombatStateText(newState);
        GameEventSystem.current.onEnemyDeath += updateEnemyRemaining;
    }

    private string GetCombatStateText(CombatState state)
    {
        Debug.Log("Changing text");
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
                return "????";
        }
    }

    private void CombatStateManagerOnOnCombatStateChanged(CombatState state)
    {
        Debug.Log("Updating button interactivity");

        //these booleans stop the buttons from being interactable post-deploy phase, given that it's the first phase in the game, naturally, it just gets disabled immediately.

        bool deployIsActive = _deployConfirmButton.gameObject.activeSelf;
        bool deployresetIsActive = _deployResetButton.gameObject.activeSelf;

        //These two lines basically say "if the current state is the player turn, then the button is interactable, otherwise, the player can't click those buttons if it's not their turn.
        if (state == CombatState.PlayerTurn)
        {
            _primeAttackButton.interactable = true;
            _endTurnButton.interactable = true;

            if (deployIsActive == true && deployresetIsActive == true)
            {
                _deployConfirmButton.gameObject.SetActive(false);
                _deployResetButton.gameObject.SetActive(false);
                _unitsLeftText.gameObject.SetActive(false);
                _unitsLeftNumber.gameObject.SetActive(false);
                _endTurnButton.gameObject.SetActive(false);
            }
            
            if (_primeAttackButton.gameObject.activeSelf == false && _endTurnButton.gameObject.activeSelf == false)
            {
                _primeAttackButton.gameObject.SetActive(true);
                _endTurnButton.gameObject.SetActive(true);
            }
        }
        else if (state == CombatState.DeployPhase)
        {
            _primeAttackButton.gameObject.SetActive(false);
            _endTurnButton.gameObject.SetActive(false);
        }
        else if (state == CombatState.OutOfCombat)
        {

        }
        else if(state != CombatState.PlayerTurn)
        {
            _primeAttackButton.interactable = false;
            _endTurnButton.interactable = false;
            _deployConfirmButton.gameObject.SetActive(false);
            _deployResetButton.gameObject.SetActive(false);
        }
    }

    //method needs to be public so that we can hook it up to the buttons
    public void EndTurn()
    {
        CombatStateManager.CSInstance.UpdateCombatState(CombatState.EnemyTurn);
    }
    
    //method to update the deployList GUI feature.
    public void updateDeployList()
    {

    }

    private void updateEnemyRemaining()
    {
        _enemyUnitsRemainingNumber.text = ObjectiveManager.OMInstance.PoCKillCounter.ToString();
    }
    //bottom text
    public void PrimeAttackButtonPressed()
    {
        //code for switching the playerUnit state to "attackPrimed" goes here.
    }

    public void AttackExecuted()
    {
        //code for executing attack durng "primeAttack" state goes here
        //side note for myself: make sure to add "isAttacking" state for player. Or just make the attack instant and set the "hasAttacked" boolean to true after.
    }

    public void resetDeployButton()
    {
        GameEventSystem.current.resetDeployPressed();
    }

    public void confirmDeploy()
    {
        CombatStateManager.CSInstance.UpdateCombatState(CombatState.PlayerTurn);
    }

    //used for later
    private void deactivateAllCombatUI()
    {
        _primeAttackButton.gameObject.SetActive(false);
        _endTurnButton.gameObject.SetActive(false);
        _deployConfirmButton.gameObject.SetActive(false);
        _deployResetButton.gameObject.SetActive(false);
        _unitsLeftText.gameObject.SetActive(false);
        _unitsLeftNumber.gameObject.SetActive(false);
        _enemyUnitsRemainingNumber.gameObject.SetActive(false);
    }
}
