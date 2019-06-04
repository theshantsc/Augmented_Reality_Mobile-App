using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


  public class HighScore : IComparable<HighScore>
{
    public string name;
    public int totalscore;


    public HighScore(string newName, int totalscore)
    {
        name = newName;
        totalscore = totalscore;
    }

      public int CompareTo(HighScore other)
    {
        if (totalscore > other.totalscore)

        { return 1; }
        if (totalscore == other.totalscore)
        { return 0; }
        return -1;
    }
}


public class StartLevel : MonoBehaviour
{

    public string levelName;
    public string nextLevel;
    private List<HighScore> highScores = new List<HighScore>();


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

       highScores.Sort();

        foreach(HighScore guy in highScores)
        {
        Debug.LogFormat("guy = {0}");
        }

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


    public void QuitApplication()
    {
        Application.Quit();
    }


          protected virtual void GetIntailDbValues(string UserId) {
          Debug.Log("GetIntailDbValues User Id : " + UserId);

        playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");

            string levelName="Level01";
           if(levelName.Trim().Equals("Level01")){
               Debug.Log("playerReadRef level01 value save{0}  test..");
                     }

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


 
       protected virtual void GetIntailScoreValues(string UserId) {
          Debug.Log("GetIntailScoreValues User Id : " + UserId);

         playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");

          playerReadRef.OrderByChild("userId").GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                        // Handle the error...
                         Debug.Log("Handle the error task.IsFaulted");
                    }
                    else if (task.IsCompleted) {
                            Debug.Log("Task Completed get User Detail :");
                             DataSnapshot snapshot = task.Result;
                              Debug.LogFormat("Key parent top = {0}", snapshot.Key); 
                                 if (snapshot != null && snapshot.ChildrenCount > 0) {
                                       int level1Score=0;
                                                      int level2Score=0;
                                                      string name="";
                                                      int totalScore=0;
                                     foreach (var childSnapshot in snapshot.Children) {
                                                   
                                                      //  IDictionary dictUser1 = (IDictionary)childSnapshot.Value;

                                                       // level1Score = int.Parse(dictUser1["score"]["level1"].ToString());
                                                       // level2Score = int.Parse(dictUser1["score"]["level2"].ToString());
                                                         //level2Score = int.Parse(childSnapshot.Child("score").Child("level2").Value;
                                                      // Debug.Log("level1Score"+level1Score);
                                                      //  Debug.Log("level2Score"+level2Score);
                                                       // name = int.Parse(dictUser1["playername"].ToString());
                                                       // Debug.Log("name"+name);
                                                       Debug.LogFormat("Key parent = {0}", childSnapshot.Key); 
                                                       IDictionary dicttop = (IDictionary)childSnapshot.Value;
                                                          name = dicttop["playername"].ToString();
                                                    IDictionary scoreslist =(IDictionary)dicttop["score"];
                                                         level1Score = int.Parse(scoreslist["level1"].ToString());
                                                                      level2Score = int.Parse(scoreslist["level2"].ToString());

                                                                      totalScore=level1Score+level1Score;
                                                                      Debug.LogFormat("totalScore = {0}", totalScore.ToString());
                                                                   

                                                        /*  foreach(var scores in scoreslist)         //levels
                                                              {
                                                             //   Debug.LogFormat("Key = {0}", scores.Key); //"Key = levelNumber"
                                                               // Debug.LogFormat("Value = {0}", scores.Value.ToString());
                                                                //levelNumber.Value.ToString()

                                                                  if(scores.Key.Equals("playername")){
                                                                     // Debug.LogFormat("name = {0}", scores.Value.ToString());
                                                                      //name=scores.Value.ToString();
                                                                   }
                                                                   if(scores.Key.Equals("score")){
                                                                     IDictionary dictScores = (IDictionary)scores.Value;
                                                                     level1Score = int.Parse(dictScores["level1"].ToString());
                                                                      level2Score = int.Parse(dictScores["level2"].ToString());

                                                                      totalScore=level1Score+level1Score;
                                                                      Debug.LogFormat("totalScore = {0}", totalScore.ToString());
                                                                   }
                                                              }   */
                                                       
                                     } 
                                       highScores.Add(new HighScore(name, totalScore));  
                                            Debug.LogFormat("Added User = {0}",name);  
                                         }
                                 }
          });
           
           
       
       }



}
