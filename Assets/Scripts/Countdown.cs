﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


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
    
     protected Firebase.Auth.FirebaseAuth auth;
    private static Firebase.Auth.FirebaseUser loggedUser = null; 
	private DatabaseReference playerDbRef;
    private DatabaseReference playerReadRef;
    private string displayName = "";
    private int logUserCurrentAchiveLevel = 1; 
     private string imageURL = "";

    // Use this for initialization
    void Start () {
        currentTime = startingTime;
        loggedUser= LoginHandler.loggedUser;
        Debug.Log("Countdonwn Start  loggedUser ");
         Debug.Log(loggedUser);
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

                   playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");
                    Debug.Log(string.Format("playerReadRef {0}...", playerReadRef));

                    if(string.Equals(levelName, "Level01")){
                    logUserCurrentAchiveLevel =2;
                    playerReadRef.Child(loggedUser.UserId).Child("achievedlevel").SetValueAsync(2);
                    playerReadRef.Child(loggedUser.UserId).Child("score").Child("level1").SetValueAsync(score);
                    }
                    else if(string.Equals(levelName, "Level02"))
                    {
                    logUserCurrentAchiveLevel =3;
                    playerReadRef.Child(loggedUser.UserId).Child("achievedlevel").SetValueAsync(3);
                        playerReadRef.Child(loggedUser.UserId).Child("score").Child("level2").SetValueAsync(score);
                    }else {
                        Debug.Log("unexpeteted level");
                    }

            }
            else
            {
                lostScreen.SetActive(true);
            }

        }
    }



}
