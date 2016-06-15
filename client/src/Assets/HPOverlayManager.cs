using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Collections;

public class HPOverlayManager : MonoBehaviour {

    public GameObject canvas;
    private RectTransform canvasRect;
    private Dictionary<IDamagable, GameObject> healthBars = new Dictionary<IDamagable, GameObject>();
    private Dictionary<string, GameObject> playerHealthBars = new Dictionary<string, GameObject>(); 
    private HashSet<IDamagable> seenDamagables = new HashSet<IDamagable>();
    private List<string> seenPlayers = new List<string>();
    public float healthBarWidth;
    public float healthBarHeight;
    public float healthBarHoverPlayers;
    public float healthBarHoverTargets;

    // Use this for initialization
    void Start () {

        canvasRect = canvas.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {

        seenDamagables.Clear();
        seenPlayers.Clear();

        GameObject playerTrackingCamera = GameObject.Find("PlayerTrackingCamera");
        if (playerTrackingCamera == null)
        {
            return;
        }

        Camera camera = playerTrackingCamera.GetComponent<Camera>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");

        foreach (GameObject player in players)
        {
            PlayerManager manager = player.GetComponent<PlayerManager>();
            if (manager == null || manager.username == null || manager.username.Equals(""))
            {
                continue;
            }
            
            Vector3 playerPos = player.transform.position;
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, playerPos);

            GameObject healthBarObj = getPlayerHealthBar(manager.username);
            if (healthBarObj == null)
            {
                healthBarObj = instantiateHealthBar();
                Debug.LogError("Adding player healthbar: " + manager.username);
                playerHealthBars.Add(manager.username, healthBarObj);
            } else
            {
                if (!healthBarObj.activeSelf)
                {
                    healthBarObj.SetActive(true);
                }
            }
            
            RectTransform healthBar = healthBarObj.GetComponent<RectTransform>();
            healthBar.anchoredPosition = screenPos - canvasRect.sizeDelta / 2f + new Vector2(0, healthBarHoverPlayers);
            Slider slider = healthBar.GetComponentInChildren<Slider>();
            slider.normalizedValue = manager.getHealth() / manager.getMaxHealth();
            seenPlayers.Add(manager.username);

        }

        foreach (GameObject target in targets)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            RectTransform healthBar = getHealthBar(damagable);
            if (damagable == null || damagable.getHealth() <= 0.0f || damagable.getHealth() == damagable.getMaxHealth())
            {
                healthBar.gameObject.SetActive(false);
                continue;
            }
            Vector3 targetPos = target.transform.position;
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, targetPos);

            if (damagable.getHealth() == damagable.getMaxHealth())
            {
                healthBar.gameObject.SetActive(false);
            } else
            {
                healthBar.gameObject.SetActive(true);
            }
            healthBar.anchoredPosition = screenPos - canvasRect.sizeDelta / 2f + new Vector2(0, healthBarHoverTargets);
            Slider slider = healthBar.GetComponentInChildren<Slider>();
            slider.normalizedValue = damagable.getHealth() / damagable.getMaxHealth();
            
        }

        cleanHealthbars();
    }


    private void cleanHealthbars()
    {

        List<string> toRemove = new List<string>();
        foreach(KeyValuePair<string, GameObject> kvp in playerHealthBars)
        {
            if (!seenPlayers.Contains(kvp.Key))
                if (kvp.Value.activeSelf)
                {
                    kvp.Value.SetActive(false);
                    Debug.LogError("Found inactive damagable, deactivating player healthbar for player: " + kvp.Key);
                    toRemove.Add(kvp.Key);
                }
        }

        foreach (string player in toRemove)
        {
            if (playerHealthBars.Remove(player)){
                Debug.LogError("Removing player healthbar: " + player);
            } else
            {
                Debug.LogError("Unexpected Behaviour! Failed to remove player healthbar that was found in the dictionary.");
            }
        }

    }

    public GameObject getPlayerHealthBar(string playerName)
    {
        GameObject healthBar = null;
        playerHealthBars.TryGetValue(playerName, out healthBar);
        return healthBar;
    }

    private RectTransform getHealthBar(IDamagable damagable)
    {
        GameObject healthBar = null;
        healthBars.TryGetValue(damagable, out healthBar);

        if (healthBar == null)
        {
            healthBar = instantiateHealthBar();
            healthBars.Add(damagable, healthBar);
        }

        
        RectTransform hpBarTransform = healthBar.GetComponent<RectTransform>();
        return hpBarTransform;
    }

    private GameObject instantiateHealthBar()
    {
        GameObject healthBar = (GameObject)Instantiate(Resources.Load("HPBar", typeof(GameObject)));
        healthBar.transform.SetParent(canvas.transform);

        Vector2 sizeDelta = canvasRect.sizeDelta;
        Vector2 sizeDeltaHalf = sizeDelta / 2f;
        RectTransform hpBarTransform = healthBar.GetComponent<RectTransform>();
        hpBarTransform.offsetMin = new Vector2(0 + sizeDeltaHalf.x - healthBarWidth / 2, 0 + sizeDeltaHalf.y - healthBarHeight / 2);
        hpBarTransform.offsetMax = new Vector2(0 - sizeDeltaHalf.x + healthBarWidth / 2, 0 - sizeDeltaHalf.y + healthBarHeight / 2);

        return healthBar;
    }
}
