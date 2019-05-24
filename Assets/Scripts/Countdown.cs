using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Countdown : MonoBehaviour {

    public float currentTime = 0f;
    public float startingTime = 10f;
    public GameObject countDown;
    public GameObject winScreen;
    public GameObject lostScreen;
    public string levelName;
    int skySpawnCount = 0;
    int score = 0;
    public int levelPassScore;
    public int levelToUnlock;


    // Use this for initialization
    void Start () {
        currentTime = startingTime;
    }
	
	// Update is called once per frame
	void Update () {

        GameObject interaction = GameObject.Find("Interaction");
        GameObject arCamera = GameObject.Find("AR Camera");

        Place place = interaction.GetComponent<Place>();
        DestroyObjects destroy = arCamera.GetComponent<DestroyObjects>();

        skySpawnCount = place.skySpawnCount;
        score = destroy.enemyKillCount;

        if (skySpawnCount > 0)
        {

                currentTime -= 1 * Time.deltaTime;
                int finalTime = (int)currentTime;
                countDown.GetComponent<UnityEngine.UI.Text>().text = finalTime.ToString();

        }

        if (currentTime <= 0  )
        {
            currentTime = 0;

            if (score >= levelPassScore)
            {
                if (PlayerPrefs.GetInt("levelReach") < levelToUnlock)
                {
                    PlayerPrefs.SetInt("levelReach", levelToUnlock);
                }
                winScreen.SetActive(true);

            }
            else
            {
                lostScreen.SetActive(true);
            }

        }
    }
}
