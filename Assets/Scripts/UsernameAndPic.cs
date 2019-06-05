using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Unity.Editor;

public class UsernameAndPic : MonoBehaviour {

    public GameObject username;
    public Image image;
    Texture2D sprites;
     WWW publicwwwimage=null;
     string picurl;
      public static  Firebase.Auth.FirebaseUser loggedUser = null; 
        	private DatabaseReference playerDbRef;
    private DatabaseReference playerReadRef;

    // Use this for initialization
    //void Start () {

    //}
	
	// Update is called once per frame
	void Update () {
		
	}

    //void Start()
    //{
     

        // StartCoroutine(FinishDownload(url));
    //    WWW www = new WWW("https://firebasestorage.googleapis.com/v0/b/softchasers-catch-me.appspot.com/o/avata.png?alt=media&token=33f08ed1-3154-49f8-892d-dfb1da8ccdce");
    //    yield return www;
     //  WWW www = new WWW(url);
    //   yield return www;



   // sprites = publicwwwimage.texture;
    //    Rect rec = new Rect(0, 0, sprites.width, sprites.height);
    // Sprite hello = Sprite.Create(sprites, rec, new Vector2(0, 0), 1);
     //   image.sprite = hello;
   // }


      void Awake()
    {
        Debug.Log("Awake");
           loggedUser= LoginHandler.loggedUser;
        string username1 = PlayerPrefs.GetString("username");
        Debug.Log("menu username1 " + username1);
       username.GetComponent<UnityEngine.UI.Text>().text = username1;

       // GetIntailDbValues(loggedUser.UserId);
    //    Debug.Log(PlayerPrefs.GetString("url"));
    //    print(PlayerPrefs.GetString("url"));
        
        //print(PlayerPrefs.GetString("urlinfo"));


      picurl = PlayerPrefs.GetString("urlinfo");
       Debug.Log("picurl: ");
        Debug.Log(picurl);
     
    }

       IEnumerator Start()
    {
        using (WWW www = new WWW(picurl))
        {
            yield return www;
            sprites = www.texture;
       Rect rec = new Rect(0, 0, sprites.width, sprites.height);
     Sprite hello = Sprite.Create(sprites, rec, new Vector2(0, 0), 1);
      image.sprite = hello;
        }
    }



    IEnumerator FinishDownload (string url) {
     WWW hs_get = new WWW(url);
     publicwwwimage =hs_get;
    yield return hs_get;
     }

      protected virtual void GetIntailDbValues(String UserId) {
          Debug.Log("GetIntailDbValues login User Id : " + UserId);
        playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");

          playerReadRef.Child(UserId).GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                        // Handle the error...
                         Debug.Log("Handle login the error task.IsFaulted");
                    }
                    else if (task.IsCompleted) {
                      Debug.Log("Task Completed login get User Detail :");
                      DataSnapshot snapshot = task.Result;
                         IDictionary dictUser1 = (IDictionary)snapshot.Value;
                           string displayName=dictUser1["playername"].ToString();

                               PlayerPrefs.SetString("urlinfo", dictUser1["profilepicuri"].ToString());
                              PlayerPrefs.SetString("username", displayName);
                                 Debug.Log ("displayName" +displayName);
                          Debug.Log ("Image URL" +dictUser1["profilepicuri"].ToString());
                    }else {
                         Debug.Log("Else condtion");
                    }
          });
 }

}
