using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TeleportManager : MonoBehaviour {

    public GameObject otherEndpoint;
    public float teleportCooldown = 0.5f;
    public KeyCode teleportKey = KeyCode.Space;
    public GameObject teleportText;
    private Text tpText; 
    private TeamManager teamManager;
    private MapManager mapManager;
    private static float lastUsed;
    private PlayerManager playerToTp;

	// Use this for initialization
	void Start () {

        teamManager = FindObjectOfType<TeamManager>();
        mapManager = FindObjectOfType<MapManager>();
        tpText = teleportText.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(lastUsed);
        if (mapManager.waitPhase || mapManager.prepPhase)
        {
            return;
        }
        float time = Time.time;
        if (playerToTp == null)
        {
            return;
        }

        if (time - lastUsed < teleportCooldown)
        {
            teleportText.SetActive(false);
            return;
        }


        if (teleportText != null)
        {
            teleportText.SetActive(true);
        }

        if (Input.GetKey(teleportKey) && otherEndpoint != null)
        {
            playerToTp.transform.position = otherEndpoint.transform.position;
            playerToTp = null;
            lastUsed = time;
        }
	}

    void OnTriggerStay(Collider other)
    {
        PlayerManager player = other.transform.GetComponent<PlayerManager>();
        if (player == null)
        {
            playerToTp = null;
        }
        if (player.getTeamId() != teamManager.blueTeamId || !player.getView().isMine)
        {
            return;
        }

        playerToTp = player;
    }

    void OnTriggerExit(Collider other)
    {
        PlayerManager player = other.transform.GetComponent<PlayerManager>();
        if (player == null)
        {
            return;
        }
        if (player.getTeamId() != teamManager.blueTeamId || !player.getView().isMine)
        {
            return;
        }

        playerToTp = null;
        teleportText.SetActive(false);
    }
}
