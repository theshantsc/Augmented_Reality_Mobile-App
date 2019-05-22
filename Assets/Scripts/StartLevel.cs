using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour {

    public string levelName;
    public string nextLevel;
    

    // Use this for initialization
    public void LevelStart () {
        SceneManager.LoadScene(levelName);

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(levelName);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

}
