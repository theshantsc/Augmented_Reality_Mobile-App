using System;
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
        AudioListener.pause = false;
        loggedUser = LoginHandler.loggedUser;
        Debug.Log("start level  Start  loggedUser ");
         Debug.Log(loggedUser);
        GetIntailDbValues(loggedUser.UserId);
       // GetIntailScoreValues(loggedUser.UserId);

     //  highScores.Sort();

     //  foreach(HighScore guy in highScores)
      //  {
      //  Debug.LogFormat("guy = {0}");
      //  }

    }

    // Use this for initialization
    public void LevelStart()
    {

          // highScores.Sort();

      //  foreach(HighScore guy in highScores)
      //  {
        // Debug.LogFormat("guy = {0}", guy.name);
        //}

        SceneManager.LoadScene(levelName);
        Time.timeScale = 1;
        AudioListener.pause = false;



    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(levelName);
        Time.timeScale = 1;
        AudioListener.pause = false;


    }


    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
        AudioListener.pause = false;


    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
        Time.timeScale = 1;
        AudioListener.pause = false;


    }


    public void OpenHelp()
    {

     Application.OpenURL("https://sites.google.com/view/softwarechasers-catchme/home/help");
          Debug.Log("is this working site help");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }


          protected virtual void GetIntailDbValues(string UserId) {
          Debug.Log("GetIntailDbValues start level  User Id : " + UserId);

        playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");

            string levelName="Level01";
           if(levelName.Trim().Equals("Level01")){
               Debug.Log("playerReadRef start level  value save{0}  test..");
                     }

        /*   playerReadRef.Child(UserId).GetValueAsync().ContinueWith(task => {
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
          }); */


        


          
 }





}
