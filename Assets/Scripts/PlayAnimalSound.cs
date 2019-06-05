using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimalSound : MonoBehaviour
{

    public AudioSource musicSource;
    public AudioClip childSound;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                BoxCollider bc = hit.collider as BoxCollider;

                if (bc != null)
                {
                    musicSource.clip = childSound;
                    musicSource.PlayOneShot(childSound);
                }
            }
        }
    }
}
