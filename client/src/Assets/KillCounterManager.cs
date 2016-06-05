using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class KillCounterManager : MonoBehaviour {

    public Text killCounterText;
    private TeamManager teamManager;

	// Use this for initialization
	void Start () {
        teamManager = FindObjectOfType<TeamManager>();
        killCounterText.text = string.Format("Red team kills: {0}\tBlue team kills: {1}", teamManager.getRedTeamKills(), teamManager.getBlueTeamKills());
    }
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(updateCounter());
	}

    private IEnumerator updateCounter()
    {
        killCounterText.text = string.Format("Red team kills: {0}\tBlue team kills: {1}", teamManager.getRedTeamKills(), teamManager.getBlueTeamKills());
        yield return new WaitForSeconds(0.1f);
    }
}
