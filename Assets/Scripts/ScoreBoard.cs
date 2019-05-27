using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreBoard : MonoBehaviour {

    public GameObject rank;
    public GameObject name;
    public GameObject score;

    //  private List<HighScores> highScores = new List<HighScore>();
    public GameObject scorePrefab;
    public Transform scoreParent;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetScore(string rank, string name, string score)
    {
        this.rank.GetComponent<Text>().text = rank;
        this.name.GetComponent<Text>().text = name;
        this.score.GetComponent<Text>().text = score;
    }

    //private void ShowScores()
    //{
    //    for(int i = 0, i < highScores.Count, i++)
    //    {
    //        GameObject scoreObj = Instantiate(scorePrefab);
    //        HighScore tmpScore = HighScore[i];
    //        scoreObj.GetComponent<ScoreBoard>().SetScore("#" + (i + 1).ToString(), tmpScore.name, tmpScore.score);
    //        scoreObj.transform.SetParent(scoreParent);
    //        scoreObj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    //    }
    //}
}
