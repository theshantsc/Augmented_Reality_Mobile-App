using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour {

    int enemyKillCount = 0;
    public GameObject totalScore;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

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
                    enemyKillCount += 1;
                    totalScore.GetComponent<UnityEngine.UI.Text>().text = enemyKillCount.ToString();
                }
            }
        }
	}
}
