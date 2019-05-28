using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class StartLevel : MonoBehaviour
{

    public string levelName;
    public string nextLevel;


         protected Firebase.Auth.FirebaseAuth auth;
    private static Firebase.Auth.FirebaseUser loggedUser = null; 
	private DatabaseReference playerDbRef;
    private DatabaseReference playerReadRef;
    private string displayName = "";
    private int logUserCurrentAchiveLevel = 1; 
     private string imageURL = "";



 void Start () {
        loggedUser= LoginHandler.loggedUser;
        Debug.Log("start level  Start  loggedUser ");
         Debug.Log(loggedUser);
        GetIntailDbValues(loggedUser.UserId);

    }

    // Use this for initialization
    public void LevelStart()
    {
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


          protected virtual void GetIntailDbValues(string UserId) {
          Debug.Log("GetIntailDbValues User Id : " + UserId);

        playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");

          playerReadRef.Child(UserId).GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                        // Handle the error...
                         Debug.Log("Handle the error task.IsFaulted");
                    }
                    else if (task.IsCompleted) {
                      Debug.Log("Task Completed get User Detail :");
                      DataSnapshot snapshot = task.Result;
                         IDictionary dictUser1 = (IDictionary)snapshot.Value;
                            logUserCurrentAchiveLevel = int.Parse(dictUser1["achievedlevel"].ToString());
                            displayName=dictUser1["playername"].ToString();
                             imageURL=dictUser1["profilepicuri"].ToString();
                          Debug.Log ("loggedUserCurrentLevel" +logUserCurrentAchiveLevel);
      

                        //if(logUserCurrentAchiveLevel==1){
                           //       level2Button.interactable = false;
                           //   }
      
                    }else {
                         Debug.Log("Else condtion");
                    }
          });
 }

}
