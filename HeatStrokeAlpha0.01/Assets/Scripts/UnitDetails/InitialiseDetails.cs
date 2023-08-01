using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InitialiseDetails : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI Movement_Range_Text;
    public TextMeshProUGUI Attack_Damage_Text;

    void Awake()
    {
        NameText.text = "Name:";
        Movement_Range_Text.text = "Movement Range:";
        Attack_Damage_Text.text = "Attack Damage:";
    }
}