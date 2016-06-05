using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamManager : MonoBehaviour {

    public int redTeamId = 1;
    public int blueTeamId = 2;

    private int redTeamKills = 0;
    private int blueTeamKills = 0;

    private int redTeamPlayers = 0;
    private int blueTeamPlayers = 0;

    public int redTeamMaxPlayers = 1;
    public int blueTeamMaxPlayers = 1;

    public GameObject redTeamMarker;
    public GameObject blueTeamMarker;

    void Start()
    {
        StartCoroutine(assignMarkers());
    }

    [PunRPC]
    public void addPlayerToTeam(int teamId)
    {

        Debug.Log("RPC successful - addPlayerToTeam");
        if (teamId == redTeamId)
        {
            redTeamPlayers++;
        }
        if (teamId == blueTeamId)
        {
            blueTeamPlayers++;
        }
    }

    [PunRPC]
    public void removePlayerFromTeam(int teamId)
    {
        Debug.Log("RPC successful - removePlayerFromTeam: " + teamId);
        if (teamId == redTeamId)
        {
            redTeamPlayers--;
        }
        if (teamId == blueTeamId)
        {
            blueTeamPlayers--;
        }
    }

    [PunRPC]
    public void addKillToTeam(int teamId)
    {
        if (teamId == redTeamId)
        {
            redTeamKills++;
        }
        if (teamId == blueTeamId)
        {
            blueTeamKills++;
        }
    }

    public int getRedTeamPlayers()
    {
        return redTeamPlayers;
    }

    public int getBlueTeamPlayers()
    {
        return blueTeamPlayers;
    }

    public int getRedTeamKills()
    {
        return redTeamKills;
    }

    public int getBlueTeamKills()
    {
        return blueTeamKills;
    }

    public bool allTeamsFull()
    {
        return (redTeamPlayers >= redTeamMaxPlayers && blueTeamPlayers >= blueTeamMaxPlayers);
    }

    void OnGUI()
    {
        GUILayout.Label("Red team players: " + redTeamPlayers);
        GUILayout.Label("Blue team players: " + blueTeamPlayers);
    }

    public IEnumerator refresh()
    {
        redTeamPlayers = 0;
        blueTeamPlayers = 0;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("Number of players: " + players.Length);
        foreach (GameObject player in players)
        {
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            int teamId = playerManager.getTeamId();
            string name = playerManager.username;
            if (playerManager.getTeamId() == redTeamId)
            {
                redTeamPlayers++;
            }
            if (playerManager.getTeamId() == blueTeamId)
            {
                blueTeamPlayers++;
            }

        }

        yield return new WaitForSeconds(1);
    }

    IEnumerator assignMarkers()
    {
        while (true)
        {
            PlayerManager[] players = FindObjectsOfType<PlayerManager>();
            foreach (PlayerManager player in players)
            {
                if (player.getTeamMarker() != null)
                {
                    continue;
                }

                if (player.teamId == blueTeamId)
                {

                    {
                        player.setTeamMarker((GameObject)Instantiate(blueTeamMarker, player.transform.position, Quaternion.identity));
                    }
                }

                if (player.teamId == redTeamId)
                {

                    {
                        player.setTeamMarker((GameObject)Instantiate(redTeamMarker, player.transform.position, Quaternion.identity));
                    }
                }
            }

            yield return new WaitForSeconds(2.0f);
        }
            
        
    }
}
