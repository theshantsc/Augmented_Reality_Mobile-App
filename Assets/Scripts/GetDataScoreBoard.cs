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
    	public static List<HighScore2> highScores2 = new List<HighScore2>();
   


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

public class HighScore2
{
	public int Score {get; set;}
	public string Name {get;set;}
  public int Rank {get; set;}

	public HighScore2(int score,string name,int rank)
	{
		this.Score= score;
		this.Name=name;
  	this.Rank=rank;
	}
}



public class GetDataScoreBoard : MonoBehaviour {

	    private List<HighScore> highScores = new List<HighScore>();
		private List<HighScore2> highScores2 = new List<HighScore2>();

    protected Firebase.Auth.FirebaseAuth auth;
    private static Firebase.Auth.FirebaseUser loggedUser = null; 
	private DatabaseReference playerDbRef;
    private DatabaseReference playerReadRef;
      public GameObject scorePrefab;
          public Transform scoreParent;
             bool MyFunctionCalled = false;


	// Use this for initialization
	IEnumerator  Start () {

		  loggedUser = LoginHandler.loggedUser;
        Debug.Log("Start Get Scoreboard");
         Debug.Log(loggedUser);
            StartCoroutine(GetIntailScoreValues());
              yield return new WaitForSeconds(1);
            StartCoroutine(showScores());
          //showScores();
		
	}
	
	// Update is called once per frame
	void Update () {
		 

           if(MyFunctionCalled==false)
             {
           Debug.Log("Update Get Scoreboard");
         MyFunctionCalled = true;
            //StartCoroutine(showScores());
          }
  
	}

      void Awake()
    {
        Debug.Log("Awake Get Scoreboard");
       
          }

	
	     private IEnumerator GetIntailScoreValues() {
          Debug.Log("GetIntailScoreValues User Id : " );
          highScores2.Clear();
         playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");

          playerReadRef.OrderByChild("totalscore").GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                        // Handle the error...
                         Debug.Log("Handle the error task.IsFaulted");
                    }
                    else if (task.IsCompleted) {
                            Debug.Log("GetIntailScoreValues task.IsCompleted :");
                             DataSnapshot snapshot = task.Result;
                              Debug.LogFormat("Key parent top = {0}", snapshot.Key); 
                                 if (snapshot != null && snapshot.ChildrenCount > 0) {
                                     Debug.LogFormat("snapshot.ChildrenCount {0}", snapshot.ChildrenCount); 
                                       int level1Score=0;
                                                      int level2Score=0;
                                                      string name="";
                                                      int totalScore=0;
                                                      int count=1;
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
                                                      totalScore = int.Parse( dicttop["totalscore"].ToString());
                                                   // IDictionary scoreslist =(IDictionary)dicttop["score"];
                                                     //    level1Score = int.Parse(scoreslist["level1"].ToString());
                                                                     // level2Score = int.Parse(scoreslist["level2"].ToString());

                                                                     // totalScore=level1Score+level1Score;
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
                                           highScores2.Add(new HighScore2( totalScore,name,count));
										                                     count++;
                                            Debug.LogFormat("Added User = {0}",name);  

                                          //  GameObject scoreObj = Instantiate(scorePrefab);
                                       //  scoreObj.GetComponent<ScoreBoard>().SetScore(count.ToString(), name, totalScore.ToString());
                                        // scoreObj.transform.SetParent(scoreParent);
                                           // scoreObj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1); 
                                           // count= count+1;           
                                     } 
                                      // highScores.Add(new HighScore(name, totalScore));  
                                      
                                         }
                                 }
          });
           
           
         yield return null;
       }
 
 

        private IEnumerator showScores (){
          Debug.Log("showScores");
         for (int i=(highScores2.Count-1) ; i >= 0 ; i--){
            Debug.Log("showScores set values"+i);
             GameObject scoreObj = Instantiate(scorePrefab);
             HighScore2 tmpScore = highScores2[i];
             scoreObj.GetComponent<ScoreBoard>().SetScore("#" + (highScores2.Count -i).ToString(), tmpScore.Name, tmpScore.Score.ToString());
               scoreObj.transform.SetParent(scoreParent);
               scoreObj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
         }
          yield return null;
       }

}
