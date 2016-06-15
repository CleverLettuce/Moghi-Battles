using UnityEngine;
using System.Collections;

public class MapManager : Photon.MonoBehaviour {

    public SpawnManager spawnManager;
    public TeamManager teamManager;
    public GameObject hpOverlayController;
    private bool requireSpawn = false;
    public bool waitPhase = true;
    public bool prepPhase = false;
    public bool fightPhase = false;
    public bool postPhase = false;
    private BattleManager battleManager;

    // Use this for initialization
    void Start()
    {
        requireSpawn = true;
        battleManager = FindObjectOfType<BattleManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (requireSpawn)
        {
            requireSpawn = false;
            spawnManager.SpawnRandom(PhotonNetwork.player.name);
            
        }

        if (battleManager.winner != 0)
        {
            if (battleManager.winner == 1)
            {
                //Debug.Log("Red team wins");
            } else
            {
                //Debug.Log("Blue team wins");
            }
        }
    }

    public void advance()
    {
        Debug.Log("Advance called");
        if (waitPhase)
        {
            prepPhase = true;
            waitPhase = false;
            return;
        }

        if (prepPhase)
        {
            prepPhase = false;
            fightPhase = true;
            return;
        }

        if (fightPhase)
        {
            fightPhase = false;
            postPhase = true;
            return;
        }
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        object teamId = 0;
        player.customProperties.TryGetValue("teamId", out teamId);
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("removePlayerFromTeam", PhotonTargets.AllBuffered, new object[] { teamId });
        }
    }
}
