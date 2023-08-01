using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnOffDetails : MonoBehaviour
{
    private GameObject Details; 
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI Movement_Range_Text;
    public TextMeshProUGUI Attack_Damage_Text;
    public string Name;
    public string Movement_Range;
    public string Attack_Damage;

  //  void Awake()
   // {
        //Details = GameObject.FindObjectOfType<InitialiseDetails>();
   // }

    void OnMouseDown()
    {
        NameText = GameObject.Find("Name").GetComponent<TextMeshProUGUI>();
        Movement_Range_Text = GameObject.Find("MovementRange").GetComponent<TextMeshProUGUI>();
        Attack_Damage_Text = GameObject.Find("AttackDamage").GetComponent<TextMeshProUGUI>();
        NameText.text = "Name:" + Name;
        Movement_Range_Text.text = "Movement Range:" + Movement_Range;
        Attack_Damage_Text.text = "Attack Damage:" + Attack_Damage;
        AudioManager.Instance.PlaySFX("Select");
    }
    /*void OnMouseExit()
    {
        AudioManager.Instance.PlaySFX("Deselect");
    }*/
}