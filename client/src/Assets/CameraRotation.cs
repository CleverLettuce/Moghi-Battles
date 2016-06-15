using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

    public float cameraRotationSpeed = 1.0f;

	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * cameraRotationSpeed);
	}
}
