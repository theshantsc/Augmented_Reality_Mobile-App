using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsernameAndPic : MonoBehaviour {

    public GameObject username;
	// Use this for initialization
	void Start () {
        string username1 = PlayerPrefs.GetString("username");
        username.GetComponent<UnityEngine.UI.Text>().text = username1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
