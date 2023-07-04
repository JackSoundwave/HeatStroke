using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicator : MonoBehaviour
{
    public static Communicator Instance;
    public bool AttackingPlayer;
    public bool AttackingDefense;

    void Start()
    {
        Instance = this;
        AttackingPlayer = false;
        AttackingDefense = false;
    }
}