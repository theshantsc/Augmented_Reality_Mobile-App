using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour {

    int enemyKillCount = 0;
    public GameObject totalScore;
    public GameObject deathEffect;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        GameObject interaction = GameObject.Find("Interaction");
        Place place = interaction.GetComponent<Place>();

        //Vector3 zPos = Camera.main.transform.forward;


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray,out hit))
            {
                BoxCollider bc = hit.collider as BoxCollider;

                if(bc != null)
                {
                   
                    Destroy(bc.gameObject);
                    Instantiate(deathEffect, bc.gameObject.transform.position, Quaternion.identity);
                    enemyKillCount += 1;
                    totalScore.GetComponent<UnityEngine.UI.Text>().text = enemyKillCount.ToString();
                }
            }
        }
	}
}
