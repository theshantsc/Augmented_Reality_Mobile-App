using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Countdown : MonoBehaviour {

    float currentTime = 0f;
    public float startingTime = 10f;
    public GameObject countDown;
    public string levelName;

    // Use this for initialization
    void Start () {
        currentTime = startingTime;
	}
	
	// Update is called once per frame
	void Update () {
        currentTime -= 1 * Time.deltaTime;
        int finalTime = (int)currentTime;
        countDown.GetComponent<UnityEngine.UI.Text>().text = finalTime.ToString();

        if (currentTime <= 0  )
        {
            currentTime = 0;
            SceneManager.LoadScene(levelName);

        }
    }
}
