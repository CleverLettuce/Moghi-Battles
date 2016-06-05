using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DesertManager : MonoBehaviour {

    private MapManager mapManager;
    public float battleDuration;
    private double startTime = 0;
    public int winner;
    private PhotonView view;
    private WallManager goalManager;
    private TeamManager teamManager;
    public GameObject battlePhaseInfo;
    private bool initialized = false;
    private bool startTimePublished;

    // Use this for initialization
    void Start()
    {

        mapManager = FindObjectOfType<MapManager>();
        view = transform.GetComponent<PhotonView>();

        GameObject angel = GameObject.Find("angelStatue");
        goalManager = angel.GetComponent<WallManager>();

        teamManager = FindObjectOfType<TeamManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mapManager.fightPhase)
        {
            return;
        }

        if (!initialized)
        {
            PhotonNetwork.room.open = false;
            PhotonNetwork.room.visible = false;
            battlePhaseInfo.SetActiveRecursively(true);
            battlePhaseInfo.GetComponent<Image>().enabled = true;
            FindObjectOfType<CounterManager>().enabled = true;
            object battleStartTime = null;
            if (PhotonNetwork.room.customProperties.TryGetValue("battleStartTime", out battleStartTime))
            {
                startTime = (double)PhotonNetwork.room.customProperties["battleStartTime"];
            }
            else
            {
                if (PhotonNetwork.isMasterClient)
                {
                    ExitGames.Client.Photon.Hashtable startTimeHashTable = new ExitGames.Client.Photon.Hashtable();
                    startTime = PhotonNetwork.time;
                    startTimeHashTable.Add("battleStartTime", startTime);
                    PhotonNetwork.room.SetCustomProperties(startTimeHashTable);
                }

            }

            if (startTime == 0)
            {
                return;
            }

            initialized = true;
        }

        if (view.isMine)
        {
            double elapsedTime = PhotonNetwork.time - startTime;

            if (elapsedTime >= battleDuration && !goalManager.isDead())
            {
                if (teamManager.getRedTeamKills() > teamManager.getBlueTeamKills())
                {
                    winner = teamManager.redTeamId;
                } else
                {
                    if (teamManager.getRedTeamKills() == teamManager.getBlueTeamKills())
                    {
                        winner = -1;
                    } else
                    {
                        winner = teamManager.blueTeamId;
                    }
                }
            }


            // in the event that the statue is destroyed in between this and the last update call and in this call it is determined that the battle phase has ran out, the win is awarded to the attacking team.
            if (goalManager.isDead() && elapsedTime < battleDuration)
            {

                winner = teamManager.redTeamId;
            }

            if (winner != 0)
            {
                view.RPC("publishWinner", PhotonTargets.AllBuffered, new object[] { winner });
            }

        }

        if (winner != 0)
        {
            battlePhaseInfo.SetActive(false);
            mapManager.advance();
        }
    }

    public double getStartTime()
    {
        return startTime;
    }

    [PunRPC]
    public void publishWinner(int winner)
    {
        Debug.Log("Winner published: " + winner);
        this.winner = winner;
    }
}
