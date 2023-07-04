using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public string name;
    void Start()
    {
        AudioManager.Instance.PlayMusic("Theme");
    }
}
