using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Level1()
    {
        Debug.Log("Level1");
        SceneManager.LoadScene("Level1");
    }
    public void Level2()
    {
        Debug.Log("Level2");
        SceneManager.LoadScene("Level2");
    }
    public void Level3()
    {
        Debug.Log("Level3");
        SceneManager.LoadScene("Level3");
    }
    public void Level4()
    {
        Debug.Log("Level4");
        SceneManager.LoadScene("Level4");
    }
    public void BackButton()
    {
        Debug.Log("BackToMainMenu");
        SceneManager.LoadScene("StartMenu");
    }
}
