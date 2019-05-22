using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreSaveDemo : MonoBehaviour {

  public static ScoreSaveDemo Instance;

    public Text score;
    public Text userName;
    public Text level;
    public Text playingLevel;
    public Button level2Button;


   //public Text level;
    //public int level;
    public static int currentplaylevel;
    protected Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseAuth otherAuth;
     private bool fetchingToken = false;
    protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
    new Dictionary<string, Firebase.Auth.FirebaseUser>();

     public Text errorMsg;
     private string displayName = "";

    private static Firebase.Auth.FirebaseUser loggedUser = null; 
    private int logUserCurrentAchiveLevel = 1; 
    private int loggedUserCurrentScore = 0; 


      const int kMaxLogSize = 16382;
  Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;


  	private DatabaseReference playerDbRef;
    private DatabaseReference playerReadRef;


    

    // Use this for initialization
   // public void LevelStart () {
       // SceneManager.LoadScene(levelName);

   // }

    // Update is called once per frame
    void Update () {
		
	}

  public void startLevel1 () {
     //StartCoroutine(setLevel(1));
     setLevelValue(1);
  }


    public void startLevel2 () {
    //StartCoroutine(setLevel(2));
    setLevelValue(2);
  }

    public void setWin () {
    string scoreval =score.text;
    if (!String.IsNullOrEmpty(score.text)) {
        Debug.Log("setWin started");
         Debug.Log("Logged In User Id :" + loggedUser.UserId);
     playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");
    Debug.Log(String.Format("playerReadRef {0}...", playerReadRef));
    if(currentplaylevel==1){
      logUserCurrentAchiveLevel =2;
      playerReadRef.Child(loggedUser.UserId).Child("achievedlevel").SetValueAsync(2);
       playerReadRef.Child(loggedUser.UserId).Child("score").Child("level1").SetValueAsync(scoreval);

      level2Button.interactable = true;

    }else if(currentplaylevel==2)
     {
       logUserCurrentAchiveLevel =3;
       playerReadRef.Child(loggedUser.UserId).Child("achievedlevel").SetValueAsync(3);
          playerReadRef.Child(loggedUser.UserId).Child("score").Child("level2").SetValueAsync(scoreval);
     }else {
          Debug.Log("unexpeteted level");
     }
       level.text="Currently Achieved Level:"+logUserCurrentAchiveLevel.ToString();
    }else {
      errorMsg.text="Score is empty!";
    }
   
  }

    public void setLose () {
    string scoreval =score.text;
    if (!String.IsNullOrEmpty(score.text)) {
        Debug.Log("setLose started");
         Debug.Log("Logged In User Id :" + loggedUser.UserId);
     playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");
    Debug.Log(String.Format("playerReadRef {0}...", playerReadRef));
     if(currentplaylevel==1){
        playerReadRef.Child(loggedUser.UserId).Child("score").Child("level1").SetValueAsync(scoreval);
     }else if(currentplaylevel==2)
     {
          playerReadRef.Child(loggedUser.UserId).Child("score").Child("level2").SetValueAsync(scoreval);
     }else {
          Debug.Log("unexpeteted level");
     }

 
    }else {
      errorMsg.text="Score is empty!";
    }
   
  }

    public void DatabaseTest () {
        SceneManager.LoadScene("scene_01");

    }

     public void Start() {

      errorMsg.text="";
      Debug.Log("Start  Score ");
      loggedUser= LoginHandler.loggedUser;
      logUserCurrentAchiveLevel=LoginHandler.loggedUserCurrentLevel;
      displayName=LoginHandler.displayName;
      level.text="Currently Achieved Level:"+logUserCurrentAchiveLevel.ToString();
      userName.text="User Name : "+displayName;
      /*  Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
      dependencyStatus = task.Result;
      if (dependencyStatus == Firebase.DependencyStatus.Available) {
        InitializeFirebase();
      } else {
        Debug.LogError(
          "Could not resolve all Firebase dependencies: " + dependencyStatus);
      }
    });  */
      
  

    if(logUserCurrentAchiveLevel==1){
         level2Button.interactable = false;
    }

     }

     void Awake()
    {
        Debug.Log("Awake Score");
  
     
    }






 public void setLevelValue(int levelValue) {
  //string the_JSON_string="{'.sv' : 'timestamp'}";
  //var test="test";
 // var result = JSON.Parse(the_JSON_string);
  Debug.Log(levelValue);

  Debug.Log("Logged In User Id :" + loggedUser.UserId);
    //Debug.Log(the_JSON_string);
    currentplaylevel=levelValue;
     playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");
    Debug.Log(String.Format("playerReadRef {0}...", playerReadRef));
    playerReadRef.Child(loggedUser.UserId).Child("currentplaylevel").SetValueAsync(levelValue);
    playingLevel.text="playing level:"+levelValue.ToString();
    //yield return null;
}



}
