using System.Collections;
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
    public GameObject uiTimer;
    public GameObject uiLevelPassScore;
    
    
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

        uiTimer.GetComponent<UnityEngine.UI.Text>().text = currentTime.ToString();
        uiLevelPassScore.GetComponent<UnityEngine.UI.Text>().text = levelPassScore.ToString();
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
                winScreen.SetActive(true);

                if (PlayerPrefs.GetInt("levelReach") < levelToUnlock)
                {
                    PlayerPrefs.SetInt("levelReach", levelToUnlock);
                }
             
                     Debug.Log("CountDown win levelName :"+levelName);
                        Debug.Log(levelName);
                      Debug.Log("CountDown win loggedUser "+loggedUser.UserId);
                     Debug.Log(loggedUser.UserId);

                   playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");
                  //  Debug.Log(string.Format("playerReadRef {0}...", playerReadRef));

                     //playerReadRef.Child(loggedUser.UserId).Child("test").SetValueAsync(4);

                    if(levelName.Trim().Equals("Level01")){
                    logUserCurrentAchiveLevel =2;
                     Debug.Log("playerReadRef level01 value save{0}...");
                   
                    playerReadRef.Child(loggedUser.UserId).Child("score").Child("level1").SetValueAsync(score);
                    playerReadRef.Child(loggedUser.UserId).Child("totalscore").SetValueAsync(score);

                    playerReadRef.Child(loggedUser.UserId).Child("score").Child("level2").GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                        // Handle the error...
                         Debug.Log(" start level  Handle the error task.IsFaulted");
                    }
                    else if (task.IsCompleted) {
                      Debug.Log("Task Completed start level  :");
                      DataSnapshot snapshot = task.Result;
                         
                      Debug.Log ("save total score level  2" +snapshot.Value);
                      int dbscore=int.Parse(snapshot.Value.ToString());
                      Debug.Log (" save total dbscore" +dbscore);
                      int total=dbscore+score;

                    playerReadRef.Child(loggedUser.UserId).Child("totalscore").SetValueAsync(total);
      
                    }else {
                         Debug.Log("Else condtion");
                    }
                 });

                    }
                    else if(levelName.Trim().Equals("Level02"))
                    {
                    Debug.Log("playerReadRef level02 value save{0}...");
                    logUserCurrentAchiveLevel =3;
                    playerReadRef.Child(loggedUser.UserId).Child("achievedlevel").SetValueAsync(3);
                    playerReadRef.Child(loggedUser.UserId).Child("score").Child("level2").SetValueAsync(score);

                    playerReadRef.Child(loggedUser.UserId).Child("score").Child("level1").GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                        // Handle the error...
                         Debug.Log(" start level  Handle the error task.IsFaulted");
                    }
                    else if (task.IsCompleted) {
                      Debug.Log("Task Completed start level  :");
                      DataSnapshot snapshot = task.Result;
                         
                      Debug.Log ("save total score level  1" +snapshot.Value);
                      int dbscore=int.Parse(snapshot.Value.ToString());
                      Debug.Log (" save total dbscore" +dbscore);
                      int total=dbscore+score;

                    playerReadRef.Child(loggedUser.UserId).Child("totalscore").SetValueAsync(total);
      
                    }else {
                         Debug.Log("Else condtion");
                    }
                 });

                    }else {
                        Debug.Log("unexpeteted level");
                    }
            }
            if (score < levelPassScore)
            
                {
             
                     

                     Debug.Log("CountDown lost levelName :"+levelName);
                     Debug.Log(levelName);
                     Debug.Log("CountDown lost loggedUser "+loggedUser.UserId);
                     Debug.Log(loggedUser.UserId);
                   

                    playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");

                    //Debug.Log(string.Format("playerReadRef {0}...", playerReadRef));
                    playerReadRef.Child(loggedUser.UserId).Child("test").SetValueAsync(3);

                    if(levelName.Trim().Equals("Level01")){
                     Debug.Log("playerReadRef level01 lost value save...");
                   
                    playerReadRef.Child(loggedUser.UserId).Child("score").Child("level1").SetValueAsync(score);
                    playerReadRef.Child(loggedUser.UserId).Child("totalscore").SetValueAsync(score);

                    
                    playerReadRef.Child(loggedUser.UserId).Child("score").Child("level2").GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                        // Handle the error...
                         Debug.Log(" start level  Handle the error task.IsFaulted");
                    }
                    else if (task.IsCompleted) {
                      Debug.Log("Task Completed start level  :");
                      DataSnapshot snapshot = task.Result;
                         
                      Debug.Log ("save total score level  2" +snapshot.Value);
                      int dbscore=int.Parse(snapshot.Value.ToString());
                      Debug.Log ("save total dbscore " +dbscore);
                      int total=dbscore+score;

                    playerReadRef.Child(loggedUser.UserId).Child("totalscore").SetValueAsync(total);
      
                    }else {
                         Debug.Log("Else condtion");
                    }
                 });

                    }
                    else if(levelName.Trim().Equals("Level02"))
                    {
                    Debug.Log("playerReadRef level02 lost value save{0}...");
                    playerReadRef.Child(loggedUser.UserId).Child("score").Child("level2").SetValueAsync(score);

                    playerReadRef.Child(loggedUser.UserId).Child("score").Child("level1").GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                        // Handle the error...
                         Debug.Log(" start level  Handle the error task.IsFaulted");
                    }
                    else if (task.IsCompleted) {
                      Debug.Log("Task Completed start level  :");
                      DataSnapshot snapshot = task.Result;
                         
                      Debug.Log ("save total score level  1" +snapshot.Value);
                      int dbscore=int.Parse(snapshot.Value.ToString());
                      Debug.Log (" save total dbscore" +dbscore);
                      int total=dbscore+score;

                    playerReadRef.Child(loggedUser.UserId).Child("totalscore").SetValueAsync(total);
      
                    }else {
                         Debug.Log("Else condtion");
                    }
                 });

                    }else {
                        Debug.Log("unexpeteted level");
                    }
                   lostScreen.SetActive(true);
            }

        }
    }



}
