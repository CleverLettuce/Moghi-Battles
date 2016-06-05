using UnityEngine;
using System.Collections;

public class PrepPhaseBlock : MonoBehaviour {

    MapManager mapManager;

    private Vector3 deadPos = Vector3.zero;
    public float fallSpeed;

    void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
    }

	void Update () {
	
        if (!mapManager.prepPhase && !mapManager.waitPhase)
        {
            if (deadPos.Equals(Vector3.zero))
            {
                deadPos = new Vector3(transform.localPosition.x, transform.localPosition.y - 10, transform.localPosition.z);
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, deadPos, fallSpeed / 1000);
        }
	}
}
