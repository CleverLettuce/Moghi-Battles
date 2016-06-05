using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CounterManager : MonoBehaviour {

    private BattleManager battleManager;
    private MapManager mapManager;
    private Text counterText;
    private bool initialized;
    public bool go = false;

	// Use this for initialization
	void Start () {
        mapManager = FindObjectOfType<MapManager>();
        GameObject battlePhaseObject = GameObject.Find("BattleController");
        battleManager = battlePhaseObject.GetComponent<BattleManager>();

        GameObject battlePhaseTextObject = GameObject.Find("BattlePhaseCounterText");
        counterText = battlePhaseTextObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {

        double startTime = battleManager.getStartTime();
        double currTime = PhotonNetwork.time;

        double elapsedTime = currTime - startTime;
        double displayTime = battleManager.battleDuration - elapsedTime;

        int rndDisplayTime = (int)Math.Round(displayTime);
        if (rndDisplayTime < 0)
        {
            rndDisplayTime = 0;
        }

        counterText.text = "" + rndDisplayTime;
	}
}
