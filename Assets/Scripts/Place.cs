using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using System;

public class Place : MonoBehaviour
{

   // public GameObject placingObject;
    //public GameObject groundPlacingObject;
   // public GameObject placementIndicator;
    private ARSessionOrigin arSessionOrigin; // Allow to interacing with the world around us
    public Pose placementGround; // describe position and the rotation of the object that we are placing
    public Pose placementSky;
    public bool groundPlacementPoseIsValid = false;
    public bool skyPlacementPoseIsValid = false;
    private Transform camera;
    int index;
    int currentIndex;
    public int skySpawnCount = 0;
    float strength = .5f;
    public GameObject[] animals;
   // public GameObject distance;
    public float spawnTime;
    public float dissapearTime;


    // Use this for initialization
    void Start()
    {
        arSessionOrigin = FindObjectOfType<ARSessionOrigin>(); //Set reference to the AR session object
        InvokeRepeating("UpdatePlacementIndicator", 1, 1);
        InvokeRepeating("PlaceObjectAR", spawnTime, spawnTime);
        camera = Camera.current.transform;


    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPosition();
        
       // UpdatePlacementIndicator();
        // && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began
        //  if (placementPoseIsValid)
        // {
        //      PlaceObjectAR();
        // }
    }

    private void PlaceObjectAR()
    {
        if (skyPlacementPoseIsValid)
        {
             GameObject gObject;
             List<Vector3> listOfSpawnPosition = new List<Vector3>();

            if (!listOfSpawnPosition.Contains(placementSky.position))
            {

                index = Random.Range(0, animals.Length);
                skySpawnCount += 1;
                Vector3 offset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
                Vector3 finalPosition = placementSky.position + offset;
                Vector3 distanceVec = finalPosition - Camera.current.transform.position;
              //  distance.GetComponent<Text>().text = distanceVec.ToString("F4");
                // placementIndicator.transform.SetPositionAndRotation(placementSky.position, placementSky.rotation);
                gObject = Instantiate(animals[index], finalPosition + offset, Quaternion.Euler(0, Random.Range(-360, 360), 0));
                listOfSpawnPosition.Add(finalPosition);
                Destroy(gObject, dissapearTime);
            }
            else
            {
                PlaceObjectAR();

            }
        }
        else
        {
         //   placementIndicator.SetActive(false);
        }
    }


    private void UpdatePlacementIndicator()
    {
        if (groundPlacementPoseIsValid)
        {
           // placementIndicator.SetActive(true);
           // placementIndicator.transform.SetPositionAndRotation(placementGround.position, placementGround.rotation);
           // Instantiate(groundPlacingObject, placementGround.position, placementGround.rotation);

        }
        else
        {
            //placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPosition()
    {


        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)); // Finding the center point of the screen
        var hitPlacesGround = new List<ARRaycastHit>();
        var hitPlacesSky = new List<ARRaycastHit>();

        arSessionOrigin.Raycast(screenCenter, hitPlacesGround, TrackableType.Planes);
        arSessionOrigin.Raycast(screenCenter, hitPlacesSky, TrackableType.FeaturePoint);


        groundPlacementPoseIsValid = hitPlacesGround.Count > 0;
        skyPlacementPoseIsValid = hitPlacesSky.Count > 0;


        if (groundPlacementPoseIsValid)
        {
            placementGround = hitPlacesGround[0].pose;

            var forwardCamera = Camera.current.transform.forward;
            var cameraBearing = new Vector3(forwardCamera.x, 0, forwardCamera.z).normalized;
            placementGround.rotation = Quaternion.LookRotation(cameraBearing);
        }

        if (skyPlacementPoseIsValid)
        {
            placementSky = hitPlacesSky[0].pose;

            var forwardCamera = Camera.current.transform.forward;
            // var cameraBearing = new Vector3(forwardCamera.x, 0, forwardCamera.z).normalized;
            Vector3 targetPosition = new Vector3(Camera.current.transform.position.x, Camera.current.transform.position.y, Camera.current.transform.position.z);
            placementSky.rotation = Quaternion.LookRotation(targetPosition);
        }
    }

}
