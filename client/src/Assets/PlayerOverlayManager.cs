using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerOverlayManager : MonoBehaviour {

    public GameObject canvas;
    private RectTransform canvasRect;
    private Dictionary<string, GameObject> playerNameplates = new Dictionary<string, GameObject>();
    private List<string> seenPlayers = new List<string>();
    private TeamManager teamManager;
    public float nameHover;
    public float nameplateWidth;
    public float nameplateHeight;

    // Use this for initialization
    void Start()
    {
        teamManager = FindObjectOfType<TeamManager>();
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

        seenPlayers.Clear();

        GameObject playerTrackingCamera = GameObject.Find("PlayerTrackingCamera");
        if (playerTrackingCamera == null)
        {
            return;
        }

        Camera camera = playerTrackingCamera.GetComponent<Camera>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            PlayerManager manager = player.GetComponent<PlayerManager>();
            if (manager == null || manager.username == null || manager.username.Equals(""))
            {
                continue;
            }

            Vector3 playerPos = player.transform.position;
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, playerPos);

            GameObject nameTextObj = null;
            playerNameplates.TryGetValue(manager.username, out nameTextObj);
            if (nameTextObj == null)
            {
                nameTextObj = instantiateNameText();
                Debug.LogError("Adding player name plate: " + manager.username);
                playerNameplates.Add(manager.username, nameTextObj);
            }
            else
            {
                if (!nameTextObj.activeSelf)
                {
                    nameTextObj.SetActive(true);
                }
            }

            RectTransform nameplate = nameTextObj.GetComponent<RectTransform>();
            nameplate.anchoredPosition = screenPos - canvasRect.sizeDelta / 2f + new Vector2(0, nameHover);
            Text text = nameplate.GetComponentInChildren<Text>();
            text.text = manager.username;
            Outline outline = nameplate.GetComponentInChildren<Outline>();
            if (manager.teamId == teamManager.redTeamId)
            {
                outline.effectColor = Color.red;
            }
            if (manager.teamId == teamManager.blueTeamId)
            {
                outline.effectColor = Color.blue;
            }
            seenPlayers.Add(manager.username);

        }

        cleanNameplates();
    }


    private void cleanNameplates()
    {

        List<string> toRemove = new List<string>();
        foreach (KeyValuePair<string, GameObject> kvp in playerNameplates)
        {
            if (!seenPlayers.Contains(kvp.Key))
                if (kvp.Value.activeSelf)
                {
                    kvp.Value.SetActive(false);
                    Debug.LogError("Found inactive damagable, deactivating player nameplate for player: " + kvp.Key);
                    toRemove.Add(kvp.Key);
                }
        }

        foreach (string player in toRemove)
        {
            if (playerNameplates.Remove(player))
            {
                Debug.LogError("Removing player nameplate: " + player);
            }
            else
            {
                Debug.LogError("Unexpected Behaviour! Failed to remove player nameplate that was found in the dictionary.");
            }
        }

    }

    private GameObject instantiateNameText()
    {
        GameObject nameplate = (GameObject)Instantiate(Resources.Load("PlayerName", typeof(GameObject)));
        nameplate.transform.SetParent(canvas.transform);

        Vector2 sizeDelta = canvasRect.sizeDelta;
        Vector2 sizeDeltaHalf = sizeDelta / 2f;
        RectTransform nameplateTransform = nameplate.GetComponent<RectTransform>();
        nameplateTransform.offsetMin = new Vector2(0 + sizeDeltaHalf.x - nameplateWidth / 2, 0 + sizeDeltaHalf.y - nameplateHeight / 2);
        nameplateTransform.offsetMax = new Vector2(0 - sizeDeltaHalf.x + nameplateWidth / 2, 0 - sizeDeltaHalf.y + nameplateHeight / 2);

        return nameplate;
    }
}
