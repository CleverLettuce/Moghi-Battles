using UnityEngine;
using System.Collections;
using System;

public class PlayerTracker : MonoBehaviour {

    public GameObject player;
    public Vector3 defaultDistanceFromPlayer = new Vector3(0, 10, -10);
    private Vector3 currentDistanceFromPlayer;

    public float zoomRate;
    public float maxZoomOut;
    public float maxZoomIn;

    private float currentZoom = 0.0f;

    // Use this for initialization
    void Start () { 
        currentDistanceFromPlayer = defaultDistanceFromPlayer;
    }
	
	// Update is called once per frame
	void Update () {
        updateZoom();
        if (player != null)
        {
            transform.position = player.transform.position + currentDistanceFromPlayer;
        }
	}

    private void updateZoom()
    {
        float scrollRate = Input.GetAxis("Mouse ScrollWheel");
        float delta = scrollRate * zoomRate;
        currentZoom += delta;
        if (currentZoom < maxZoomOut)
        {
            currentZoom = maxZoomOut;
            delta = 0.0f;
        }
        if (currentZoom > maxZoomIn)
        {
            currentZoom = maxZoomIn;
            delta = 0.0f;
        }
        currentDistanceFromPlayer += new Vector3(0, -delta, delta);
    }
}
