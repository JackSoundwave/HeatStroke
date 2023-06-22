using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateManager : MonoBehaviour
{
    //GameState manager, but strictly for combat, so it's a turn-based script.
    public static CombatStateManager CSInstance;

    public CombatState State;

    //This is when the combatstate in the scene changes, which is what the OnCombatStateChanged variable holds
    public static event Action<CombatState> OnCombatStateChanged;

    private void Awake()
    {
        CSInstance = this;
    }

    private void Start()
    {
        UpdateCombatState(CombatState.DeployPhase);
    }

    public void UpdateCombatState(CombatState newState)
    {
        State = newState;

        switch (newState)
        {
            case CombatState.DeployPhase:
                break;
            case CombatState.PlayerTurn:
                HandlePlayerTurn();
                break;
            case CombatState.EnemyTurn:
                HandleEnemyTurn();
                break;
            case CombatState.Victory:
                break;
            case CombatState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnCombatStateChanged?.Invoke(newState);
    }

    private void HandlePlayerTurn()
    {

    }

    private void HandleEnemyTurn()
    {

    }
}

public enum CombatState
{
    DeployPhase,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Lose
}
