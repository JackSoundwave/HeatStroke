using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bestiary : MonoBehaviour
{
    public GameObject KTCButton;
    public GameObject BCButton;
    public GameObject YAButton;
    public GameObject BacButton;
    public GameObject PtzButton;
    public GameObject VrsButton;

    public void BackButton()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void KillerTCellButton()
    {
        KTCButton.SetActive(true);
        BCButton.SetActive(false);
        YAButton.SetActive(false);
        BacButton.SetActive(false);
        PtzButton.SetActive(false);
        VrsButton.SetActive(false);
    }
    public void BCellButton()
    {
        KTCButton.SetActive(false);
        BCButton.SetActive(true);
        YAButton.SetActive(false);
        BacButton.SetActive(false);
        PtzButton.SetActive(false);
        VrsButton.SetActive(false);
    }

    public void YAntibodyButton()
    {
        KTCButton.SetActive(false);
        BCButton.SetActive(false);
        YAButton.SetActive(true);
        BacButton.SetActive(false);
        PtzButton.SetActive(false);
        VrsButton.SetActive(false);
    }
    public void BacteriaButton()
    {
        KTCButton.SetActive(false);
        BCButton.SetActive(false);
        YAButton.SetActive(false);
        BacButton.SetActive(true);
        PtzButton.SetActive(false);
        VrsButton.SetActive(false);
    }
    public void ProtozoaButton()
    {
        KTCButton.SetActive(false);
        BCButton.SetActive(false);
        YAButton.SetActive(false);
        BacButton.SetActive(false);
        PtzButton.SetActive(true);
        VrsButton.SetActive(false);
    }
    public void VirusButton()
    {
        KTCButton.SetActive(false);
        BCButton.SetActive(false);
        YAButton.SetActive(false);
        BacButton.SetActive(false);
        PtzButton.SetActive(false);
        VrsButton.SetActive(true);
    }
}
