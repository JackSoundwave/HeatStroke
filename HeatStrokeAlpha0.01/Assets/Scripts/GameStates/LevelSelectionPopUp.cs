using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionPopUp : MonoBehaviour
{
    public GameObject Level1PopUp;
    public GameObject Level2PopUp;
    public GameObject Level3PopUp;
    public GameObject Level4PopUp;
    public void Level1()
    {
        Debug.Log("Level1PopUp");
        Level1PopUp.SetActive(true);
    }
    public void Level2()
    {
        Debug.Log("Level2PopUp");
        Level2PopUp.SetActive(true);
    }
    public void Level3()
    {
        Debug.Log("Level3PopUp");
        Level3PopUp.SetActive(true);
    }
    public void Level4()
    {
        Debug.Log("Level4PopUp");
        Level4PopUp.SetActive(true);
    }
    public void BackButton()
    {
        Level1PopUp.SetActive(false);
        Level2PopUp.SetActive(false);
        Level3PopUp.SetActive(false);
        Level4PopUp.SetActive(false);
    }
}
