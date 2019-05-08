using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour {

    public string levelName;

    // Use this for initialization
    public void LevelStart () {
        SceneManager.LoadScene(levelName);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
