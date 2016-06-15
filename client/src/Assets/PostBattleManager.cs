using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PostBattleManager : MonoBehaviour {

    private MapManager mapManager;
    private TeamManager teamManager;
    private BattleManager battleManager;
    private Text text;

    public GameObject postBattleInfoText;
    public GameObject postBattleInfo;

    public bool reported = false;

	void Start () {

        mapManager = FindObjectOfType<MapManager>();
        battleManager = FindObjectOfType<BattleManager>();
        teamManager = FindObjectOfType<TeamManager>();
        text = postBattleInfoText.GetComponent<Text>();

    }
	
	void Update () {
	
        if (!mapManager.postPhase)
        {
            return;
        }
        if (battleManager.winner == 0)
        {
            return;
        }

        if (!postBattleInfo.activeSelf)
        {
            postBattleInfo.SetActive(true);
        }

        if (battleManager.winner == teamManager.blueTeamId)
        {
            text.text = "Blue team wins!";
        }

        if (battleManager.winner == teamManager.redTeamId)
        {
            text.text = "Red team wins!";
        }

        if (battleManager.winner == -1)
        {
            text.text = "Draw!";
        }
        if (reported)
        {
            return;
        }
        int score = 0;
        
        PlayerManager[] players = FindObjectsOfType<PlayerManager>();
        foreach (PlayerManager player in players)
        {
            score = calculateScore(player, battleManager.winner);
            if (player.view.isMine)
            {
                bool winner = player.teamId == battleManager.winner;
                Debug.Log("Reporting for game: " + (int)PhotonNetwork.room.customProperties["gameId"]);
                Debug.Log("Token: " + LoginManager.getToken());
                StartCoroutine(doReport(LoginManager.getToken(), (int)PhotonNetwork.room.customProperties["gameId"], score, winner ? "true" : "false"));
            }
        }

        reported = true;
    }

    private int calculateScore(PlayerManager player, int winner)
    {
        return player.kills * 10 + player.teamId == winner ? 100 : 50;
    }

    private IEnumerator doReport(string token, int gameId, int score, string winner)
    {
        WWWForm reportForm = new WWWForm();
        reportForm.AddField("token", LoginManager.getToken());
        reportForm.AddField("gameId", (int)PhotonNetwork.room.customProperties["gameId"]);
        reportForm.AddField("score", score);
        reportForm.AddField("winner", winner);
        Debug.Log("Score: " + score);
        WWW report = new WWW(DataServerDomain.url + "report", reportForm.data);
        yield return report;
        Debug.Log(report.text);
    }
}
