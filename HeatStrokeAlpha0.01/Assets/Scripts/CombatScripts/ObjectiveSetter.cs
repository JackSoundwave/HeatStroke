using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class ObjectiveSetter : MonoBehaviour
{
    public Objective obj;


    [SerializeField]
    private int hivesToDestroy, enemiesKilled, turnTimer, PoCKillCounter;

    private void Start()
    {
        switch (obj)
        {
            case Objective.Defense:
                ObjectiveManager.OMInstance.setObjective(obj);
                ObjectiveManager.OMInstance.setTurnTimer(turnTimer);
                break;

            case Objective.Extermination:
                ObjectiveManager.OMInstance.setObjective(obj);
                ObjectiveManager.OMInstance.setTurnTimer(turnTimer);
                ObjectiveManager.OMInstance.setHivesToDestroy(hivesToDestroy);
                break;

            case Objective.Escort:
                ObjectiveManager.OMInstance.setObjective(obj);
                ObjectiveManager.OMInstance.setTurnTimer(turnTimer);
                //tba
                break;

            case Objective.PoC_Extermination:
                ObjectiveManager.OMInstance.setObjective(obj);
                ObjectiveManager.OMInstance.setTurnTimer(turnTimer);
                ObjectiveManager.OMInstance.setPoCKillCount(PoCKillCounter);
                break;

            default:
                Debug.Log("ERROR: MORON DID NOT SET PROPER OBJECTIVE TYPE");
                throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
        }
    }
}
