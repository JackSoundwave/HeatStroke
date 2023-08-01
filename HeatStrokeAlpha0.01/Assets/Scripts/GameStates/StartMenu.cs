using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    public GameObject StartMenuUI;
    public string levelSelect;

    public void Play()
    {
        SceneManager.LoadScene(levelSelect);
    }

    public void Options()
    {
        SceneManager.LoadScene("Option");
    }
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}