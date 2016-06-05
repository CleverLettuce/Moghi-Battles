using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PrepCounterManager : MonoBehaviour {

    public GameObject prepPhaseTextObject;

    private PrepPhaseManager prepManager;
    private MapManager mapManager;
    private Text counterText;

    // Use this for initialization
    void Start()
    {
        mapManager = FindObjectOfType<MapManager>();

        GameObject prepPhaseObject = GameObject.Find("PrepPhaseController");
        prepManager = prepPhaseObject.GetComponent<PrepPhaseManager>();

        counterText = prepPhaseTextObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        double startTime = prepManager.getStartTime();
        double currTime = PhotonNetwork.time;

        double elapsedTime = currTime - startTime;
        double displayTime = prepManager.prepPhaseDuration - elapsedTime;

        int rndDisplayTime = (int)Math.Round(displayTime);
        if (rndDisplayTime < 0)
        {
            rndDisplayTime = 0;
        }

        counterText.text = "" + rndDisplayTime;
    }
}
