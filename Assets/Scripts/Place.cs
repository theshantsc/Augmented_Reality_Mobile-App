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

    public GameObject placingObject;
    public GameObject groundPlacingObject;
    public GameObject placementIndicator;
    private ARSessionOrigin arSessionOrigin; // Allow to interacing with the world around us
    private Pose placement; // describe position and the rotation of the object that we are placing
    private bool placementPoseIsValid = false;

    // Use this for initialization
    void Start()
    {
        arSessionOrigin = FindObjectOfType<ARSessionOrigin>(); //Set reference to the AR session object
        InvokeRepeating("PlaceObjectAR", 3, 3);
        InvokeRepeating("UpdatePlacementIndicator", 1, 1);


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

        Vector3 zPos = Camera.main.transform.forward ;
       // zPos.y = 0;
     //   zPos = Quaternion.AngleAxis(Random.Range(-45, 45), Vector3.up) * zPos;
        Instantiate(placingObject, zPos, Quaternion.identity);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            // placementIndicator.SetActive(true);
            Vector3 zPos = Camera.main.transform.forward;

            placementIndicator.transform.SetPositionAndRotation(placement.position, placement.rotation);
            Instantiate(placingObject, placement.position, placement.rotation);

        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPosition()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(.5f, .5f)); // Finding the center point of the screen
        var hitPlaces = new List<ARRaycastHit>();
        arSessionOrigin.Raycast(screenCenter, hitPlaces, TrackableType.FeaturePoint);

        placementPoseIsValid = hitPlaces.Count > 0;

        if (placementPoseIsValid)
        {
            placement = hitPlaces[0].pose;

            var forwardCamera = Camera.current.transform.forward;
            var cameraBearing = new Vector3(forwardCamera.x, 0, forwardCamera.z).normalized;
            placement.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
