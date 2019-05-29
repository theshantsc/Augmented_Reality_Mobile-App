using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UsernameAndPic : MonoBehaviour {

    public GameObject username;
    public Image image;
    Texture2D sprites;
    // Use this for initialization
    //void Start () {

    //}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Start()
    {
        string username1 = PlayerPrefs.GetString("username");
        username.GetComponent<UnityEngine.UI.Text>().text = username1;

        Debug.Log(PlayerPrefs.GetString("url"));
        print(PlayerPrefs.GetString("url"));

        string url = PlayerPrefs.GetString("url");

        WWW www = new WWW("https://firebasestorage.googleapis.com/v0/b/softchasers-catch-me.appspot.com/o/avata.png?alt=media&token=33f08ed1-3154-49f8-892d-dfb1da8ccdce");
        yield return www;


        sprites = www.texture;
        Rect rec = new Rect(0, 0, sprites.width, sprites.height);
        Sprite hello = Sprite.Create(sprites, rec, new Vector2(0, 0), 1);
        image.sprite = hello;
    }
}
