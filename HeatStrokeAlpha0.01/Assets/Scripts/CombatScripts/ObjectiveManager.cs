using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    //This is to keep track of objectives being completed/failed in the game. Also dictates what kind of mission it currently is.
    //We could also refactor it later to include side objectives.
    public static ObjectiveManager OMInstance;

    public Objective obj;

    private void Awake()
    {
        OMInstance = this;
    }

    private void Start()
    {
      
    }

    public void UpdateObjectiveDetails()
    {

    }
}

public enum Objective
{
    Extermination,
    Defense,
    Train,
    Escort,
}