using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class SpawnManager : MonoBehaviour {

    public GameObject[] redTeamSpawnSpots;
    public GameObject[] blueTeamSpawnSpots;

    private TeamManager teamManager;
    private PhotonView photonView;

    // Use this for initialization
    void Start () {

        teamManager = transform.GetComponent<TeamManager>();
        photonView = transform.GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnRandom (string username)
    {
        Debug.Log("Spawning player: " + username);
        int redTeamPlayers = teamManager.getRedTeamPlayers();
        int blueTeamPlayers = teamManager.getBlueTeamPlayers();

        Debug.Log("Red team players: " + redTeamPlayers);
        Debug.Log("Blue team players: " + blueTeamPlayers);

        if (redTeamPlayers >= teamManager.redTeamMaxPlayers && blueTeamPlayers >= teamManager.blueTeamMaxPlayers)
        {
            Debug.LogError("Too many players in the room!");
            return;
        }

        if (redTeamPlayers == blueTeamPlayers)
        {
            int selector = Random.Range(1, 3);
            GameObject[] spawnSpots;
            if (selector == 1)
            {
                spawnSpots = redTeamSpawnSpots;
            } else
            {
                spawnSpots = blueTeamSpawnSpots;
            }
            spawn(HeroSelector.hero, spawnSpots, selector, username);
        } else
        {
            if (redTeamPlayers < blueTeamPlayers)
            {
                spawn(HeroSelector.hero, redTeamSpawnSpots, teamManager.redTeamId, username);
            } else
            {
                spawn(HeroSelector.hero, blueTeamSpawnSpots, teamManager.blueTeamId, username);
            }
        }
    }

    void spawn(string prefab, GameObject[] spawnSpots, int teamId, string username)
    {
        GameObject spawnSpot = selectSpawnSpot(spawnSpots);
        Debug.Log("Spawning player");
        GameObject player = PhotonNetwork.Instantiate(prefab, spawnSpot.transform.position, spawnSpot.transform.rotation, 0);
        player.GetComponent<PlayerManager>().teamId = teamId;
        Camera.main.enabled = false;
        ((MonoBehaviour)player.GetComponent("EightDirMovement")).enabled = true;
        ((MonoBehaviour)player.GetComponent("SkillManager")).enabled = true;
        // player.GetComponent("CharacterController");
        GameObject playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera");
        playerCamera.SetActive(true);
        PlayerTracker tracker = playerCamera.GetComponent<PlayerTracker>();
        tracker.player = player;
        DepthOfField dof = playerCamera.GetComponent<DepthOfField>();
        if (dof != null && dof.isActiveAndEnabled)
        {
            dof.focalTransform = player.transform;
        }
        player.layer = 8;
        Hashtable playerTeamHashTable = new Hashtable();
        playerTeamHashTable["teamId"] = teamId;
        PhotonNetwork.player.SetCustomProperties(playerTeamHashTable);
        Debug.Log("Player team: " + PhotonNetwork.player.customProperties["teamId"]);
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        playerManager.username = username;
        playerManager.myTeamSpawns = spawnSpots;
        playerManager.teamId = teamId;
        //player.transform.FindChild("PlayerCamera").gameObject.SetActive(true);
        photonView.RPC("addPlayerToTeam", PhotonTargets.AllBuffered, teamId);
    }

    private GameObject selectSpawnSpot(GameObject[] spawnSpots)
    {
        return spawnSpots[Random.Range(0, spawnSpots.Length)];
    }
}
