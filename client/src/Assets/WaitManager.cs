using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaitManager : MonoBehaviour {

    public GameObject gameController;
    public GameObject waitPanel;
    public int requiredPlayers;
    private MapManager mapManager;
    private TeamManager teamManager;

	// Use this for initialization
	void Start () {

        teamManager = gameController.GetComponent<TeamManager>();
        mapManager = gameController.GetComponent<MapManager>();
        StartCoroutine(checkPlayerCount());
    }
	
	// Update is called once per frame
	void Update () {
        if (!mapManager.waitPhase)
        {
            return;
        }

        
	}

    IEnumerator checkPlayerCount()
    {
        while (true)
        {
            PlayerManager[] players = FindObjectsOfType<PlayerManager>();
            HashSet<string> playerSet = new HashSet<string>();
            foreach (PlayerManager player in players)
            {
                if (player.username == "")
                {
                    continue;
                }
                playerSet.Add(player.username);
            }

            if (PhotonNetwork.room != null && PhotonNetwork.room.playerCount >= requiredPlayers && playerSet.Count >= requiredPlayers)
            {
                Debug.Log("Calling advance");
                mapManager.advance();
                waitPanel.SetActive(false);
                break;
            }

            yield return new WaitForSeconds(0.3f);
        }
        
    }
}
