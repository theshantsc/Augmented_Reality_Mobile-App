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
    private ARSessionOrigin arSessionOrigin; // Allow to interacing with the world around us
    public Pose placementSky; // position and rotation of the 3d point
    public bool skyPlacementPoseIsValid = false;
    private Pose placement;
    public bool placementPoseIsValid = false;
    private Transform camera;
    int index;
    int currentIndex;
    public int skySpawnCount = 0;
    float strength = .5f;
    public GameObject[] animals;
   // public GameObject distance;
    public float spawnTime;
    public float dissapearTime;
    public string levelName;
    public GameObject placementIndicator;
    bool isObjectPlaced = false;
    public GameObject[] animalInfo;
    public GameObject findSurfaceText;
    public GameObject tapText;



    // Use this for initialization
    void Start()
    {
        arSessionOrigin = FindObjectOfType<ARSessionOrigin>(); //Set reference to the AR session object

        if ( !(levelName == "InfoLevel"))
        {
            InvokeRepeating("UpdatePlacementIndicator", 1, 1);
            InvokeRepeating("PlaceObjectAR", spawnTime, spawnTime);
        }

        camera = Camera.current.transform;

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPosition();


        UpdatePlacementIndicator();

        if (levelName == "InfoLevel")
        {
            if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && isObjectPlaced == false)
            {
                PlaceInfoAnimal();
                isObjectPlaced = true;
                findSurfaceText.SetActive(false);
                tapText.SetActive(false);
            }
         }

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
               // Vector3 distanceVec = finalPosition - Camera.current.transform.position;
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

    private void PlaceInfoAnimal()
    {
        Instantiate(animalInfo[0], placement.position, placement.rotation);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid && isObjectPlaced == false)
        {
            placementIndicator.SetActive(true);
            tapText.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placement.position, placement.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
            tapText.SetActive(false);
        }
    }

    private void UpdatePlacementPosition()
    {


        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)); // Finding the center point of the screen
        var hitPlaces = new List<ARRaycastHit>();
        var hitPlacesSky = new List<ARRaycastHit>();
        arSessionOrigin.Raycast(screenCenter, hitPlacesSky, TrackableType.FeaturePoint); // send a ray from some point on the screen straight out to the real world 
        arSessionOrigin.Raycast(screenCenter, hitPlaces, TrackableType.FeaturePoint);


        placementPoseIsValid = hitPlaces.Count > 0; // Check if ray hits one or more than one items

        skyPlacementPoseIsValid = hitPlacesSky.Count > 0; // Check if ray hits one or more than one items

        if (placementPoseIsValid)
        {
            placement = hitPlaces[0].pose;
            var forwardCamera = Camera.current.transform.forward; // describes the direction that camera facing along the x,y and z axis
            var cameraBearing = new Vector3(forwardCamera.x, 0, forwardCamera.z).normalized;
            placement.rotation = Quaternion.LookRotation(cameraBearing);
        }

        if (skyPlacementPoseIsValid)
        {
            placementSky = hitPlacesSky[0].pose;
            var forwardCamera = Camera.current.transform.forward;
            //var cameraBearing = new Vector3(forwardCamera.x, 0, forwardCamera.z).normalized;
            Vector3 targetPosition = new Vector3(Camera.current.transform.position.x, Camera.current.transform.position.y, Camera.current.transform.position.z);
            placementSky.rotation = Quaternion.LookRotation(targetPosition);
        }
    }

    public void Next()
    {

        animalInfo[0].SetActive(false);
        animalInfo[1].SetActive(true);
        
    }
    public void Back()
    {
        animalInfo[1].SetActive(false);
        animalInfo[0].SetActive(true);
    }

}
