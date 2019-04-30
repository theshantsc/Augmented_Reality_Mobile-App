﻿using System.Collections;
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
    public GameObject placementIndicator;
    private ARSessionOrigin arSessionOrigin;
    private Pose placement;
    private bool placementPoseIsValid = false;

    // Use this for initialization
    void Start()
    {
        arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
        InvokeRepeating("PlaceObjectAR", 3, 3);

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPosition();
        UpdatePlacementIndicator();
        // && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began
        //  if (placementPoseIsValid)
        // {
        //      PlaceObjectAR();
        // }
    }

    private void PlaceObjectAR()
    {

        Vector3 zPos = Camera.current.transform.forward ;
        zPos.y = 0;
     //   zPos = Quaternion.AngleAxis(Random.Range(-45, 45), Vector3.up) * zPos;
        Instantiate(placingObject, zPos, Quaternion.identity);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placement.position, placement.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPosition()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(.5f, .5f));
        var hitPlaces = new List<ARRaycastHit>();
        arSessionOrigin.Raycast(screenCenter, hitPlaces, TrackableType.Planes);

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
