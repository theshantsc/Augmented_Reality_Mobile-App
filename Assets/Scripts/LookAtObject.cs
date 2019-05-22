using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour {

   public GameObject target;
	// Use this for initialization
	void Start () {
        target = GameObject.Find("Cube");
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(target.transform.position);
	}
}
