using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float zoomRate;
    public float maxZoomOut;
    public float maxZoomIn;

    private float currentZoom = 0.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
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
        transform.position = new Vector3(transform.position.x, transform.position.y - delta, transform.position.z + delta);
    }

}
