using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//This is to debug the punchingBagMovement script

[CustomEditor(typeof(PunchingBagMovement))]
public class PunchingBagMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PunchingBagMovement punchingBagMovement = (PunchingBagMovement)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Trigger Enemy Movement"))
        {
            punchingBagMovement.TriggerEnemyMovement();
        }
    }
}
