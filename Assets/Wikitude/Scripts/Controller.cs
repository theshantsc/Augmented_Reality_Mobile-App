using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Wikitude;

public class Controller : MonoBehaviour {

    public InstantTracker Tracker;
    public Button StateButton;
    public Text StateButtonText;
    public Text MessageBox;
    public GameObject ARPrefab;
    public GameObject enemyPrefab;

    GridRenderer grid;
    InstantTrackable trackable;

    bool isSpawning = false;
    bool isTracking = false;

    InstantTrackingState trackerState = InstantTrackingState.Initializing;
    bool isChanging = false;

    void Awake()
    {
        grid = GetComponent<GridRenderer>();
        grid.enabled = true;
        trackable = Tracker.GetComponent<InstantTrackable>();
    }

    public void OnInitializationStarted(InstantTarget target)
    {
        SetSceneEnabled(true);
    }

    public void OnInitializationStopped(InstantTarget target)
    {
        SetSceneEnabled(false);
    }

    public void OnSceneRecognized(InstantTarget target)
    {
        SetSceneEnabled(true);
        isTracking = true;
        MessageBox.text = "Scene Found";
    }

    public void OnSceneLost(InstantTarget target)
    {
        SetSceneEnabled(false);
        isTracking = false;
        MessageBox.text = "Scene Lost";
    }

    void SetSceneEnabled(bool enabled)
    {
        grid.enabled = enabled;

        if (enabled)
        {
            InvokeRepeating("SpawnEnemy", 1, 1);

        }
        else
        {
            CancelInvoke("SpawnEnemy");
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("sphere");

        foreach(GameObject g in gos)
        {
            Renderer[] rends = g.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rends)
                r.enabled = enabled;

        }

        gos = GameObject.FindGameObjectsWithTag("zombie");

        foreach (GameObject g in gos)
        {
            Renderer[] rends = g.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rends)
                r.enabled = enabled;

        }
    }

    public void StateButtonPressed()
    {
        isSpawning = true;

        if (!isChanging)
        {
            if (trackerState == InstantTrackingState.Initializing)
            {
                if (Tracker.CanStartTracking())
                {
                    StateButtonText.text = "Switching State...";
                    isChanging = true;
                    Tracker.SetState(InstantTrackingState.Tracking);
                    grid.enabled = false;
                }
            }

            else
            {
                StateButtonText.text = "Switching State...";
                isChanging = true;
                Tracker.SetState(InstantTrackingState.Initializing);
                grid.enabled = true;
            }
        }
    }

    public void OnStateChanged(InstantTrackingState newState)
    {
        trackerState = newState;

        if (trackerState == InstantTrackingState.Initializing)
        {
            StateButtonText.text = "Start Tracking";
            MessageBox.text = "Not Tracking";
        }
        else
        {
            StateButtonText.text = "Stop Tracking";
            MessageBox.text = "Tracking";
        }
        isChanging = false;
    }

    public void OnHeightValueChanged(float heightValue)
    {
        Tracker.DeviceHeightAboveGround = heightValue;
    }

	// Use this for initialization
	void Start () {
        MessageBox.text = "Starting the SDK";
	}
	
    void SpawnEnemy()
    {
        if (!isSpawning) return;

        Vector3 zPos = Camera.main.transform.forward * 10;
        zPos.y = 0;
        zPos = Quaternion.AngleAxis(Random.Range(-45, 45), Vector3.up )* zPos;
        Instantiate(enemyPrefab, zPos, Quaternion.identity);
    }
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0) && isTracking && !EventSystem.current.IsPointerOverGameObject())
        {
            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            UnityEngine.Plane groundPlane = new UnityEngine.Plane(Vector3.up, Vector3.zero);
            float touchPos;

            if(groundPlane.Raycast(cameraRay, out touchPos))
            {
                Vector3 position = cameraRay.GetPoint(touchPos) + new Vector3(0,0.1f,0);
                GameObject newAR = Instantiate(ARPrefab, position, Quaternion.identity);
                newAR.transform.parent = trackable.transform;
                newAR.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 60);
            }
        }



        if (trackerState == InstantTrackingState.Initializing)
        {
            if (Tracker.CanStartTracking())
            {
                grid.TargetColor = Color.green;
            }else
            {
                grid.TargetColor = GridRenderer.DefaultTargetColor;
            }
        }
        else
        {
            grid.TargetColor = GridRenderer.DefaultTargetColor;
        }
	}
}
