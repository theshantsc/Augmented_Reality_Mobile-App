using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeLevel : MonoBehaviour {

    public GameObject resumeScreen;

    public void DisplayResumeScreen()
    {
        if(resumeScreen.activeSelf == false)
        {
            resumeScreen.SetActive(true);
        }else
        {
            resumeScreen.SetActive(false);
        }


        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        
    }
}
