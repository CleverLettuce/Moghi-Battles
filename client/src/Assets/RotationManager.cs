using UnityEngine;
using System.Collections;

public class RotationManager : MonoBehaviour {

    public float speed;
    public float angle;
    private Vector3 startRotation;
    private Vector3 to;
	
    void Start()
    {
        startRotation = transform.rotation.eulerAngles;
        to = startRotation + new Vector3(0, angle, 0);
    }

	// Update is called once per frame
	void Update () {
	
        if (Quaternion.Angle(transform.rotation, Quaternion.Euler(to)) <=2)
        {
            Vector3 t = to;
            to = startRotation;
            startRotation = t;
        }
        transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(transform.rotation.eulerAngles), Quaternion.Euler(to), speed*Time.deltaTime);

	}
}
