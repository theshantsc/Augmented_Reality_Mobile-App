using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour {

    public int enemyKillCount = 0;
    public GameObject Score;
    public GameObject deathEffect;
    public string levelName;
    public GameObject winScore;
    public GameObject loseScore;
    public AudioClip hitSound;
    public AudioClip childSound;
    public AudioSource musicSource;
    public AudioSource audioSource;
    public GameObject unmuteBtn;
    public GameObject muteBtn;
    public GameObject unmuteMusicBtn;
    public GameObject muteMusicBtn;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {

        GameObject interaction = GameObject.Find("Interaction");
        Place place = interaction.GetComponent<Place>();

        GameObject camera = GameObject.Find("AR Camera");
        Countdown count = camera.GetComponent<Countdown>();
        //Vector3 zPos = Camera.main.transform.forward;


        if (count.currentTime > 0) {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    BoxCollider bc = hit.collider as BoxCollider;

                    if (bc != null)
                    {
                        if (levelName == "InfoLevel")
                        {
                            musicSource.clip = childSound;
                            musicSource.PlayOneShot(childSound);
                        }
                        else
                        {
                            musicSource.clip = hitSound;
                            musicSource.PlayOneShot(hitSound);

                            Destroy(bc.gameObject);
                            Instantiate(deathEffect, bc.gameObject.transform.position, Quaternion.identity);
                            enemyKillCount += 1;

                            PlayerPrefs.SetInt(levelName, enemyKillCount);

                            int totalScore = PlayerPrefs.GetInt("Level01") + PlayerPrefs.GetInt("Level02");

                            Score.GetComponent<UnityEngine.UI.Text>().text = enemyKillCount.ToString();
                            winScore.GetComponent<UnityEngine.UI.Text>().text = totalScore.ToString();
                            loseScore.GetComponent<UnityEngine.UI.Text>().text = totalScore.ToString();


                        }
                    }
                }
            }
        }
    }

    public void MuteMusic()
    {
        AudioListener.pause = true;
        muteMusicBtn.SetActive(false);
        unmuteMusicBtn.SetActive(true);

    }

    public void UnuteMusic()
    {
        AudioListener.pause = false;
        unmuteMusicBtn.SetActive(false);
        muteMusicBtn.SetActive(true);
    }

}
