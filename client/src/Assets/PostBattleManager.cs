using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PostBattleManager : MonoBehaviour {

    private MapManager mapManager;
    private TeamManager teamManager;
    private BattleManager battleManager;
    private Text text;

    public GameObject postBattleInfoText;
    public GameObject postBattleInfo;

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
    }
}
