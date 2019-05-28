using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{

    public string levelName;
    public string nextLevel;


    // Use this for initialization
    public void LevelStart()
    {
        SceneManager.LoadScene(levelName);
        Time.timeScale = 1;


    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(levelName);
        Time.timeScale = 1;

    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;

    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
        Time.timeScale = 1;

    }

    public void QuitApplication()
    {
        Application.Quit();
    }

}
