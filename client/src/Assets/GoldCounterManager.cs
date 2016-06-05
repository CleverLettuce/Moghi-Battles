using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoldCounterManager : MonoBehaviour {

    public PlayerManager playerManager;
    public Text text;

    void Start()
    {
        text = transform.GetComponent<Text>();
    }

	// Update is called once per frame
	void Update () {

        if (playerManager != null)
        {
            text.text = "Gold: " + Mathf.RoundToInt(playerManager.gold);
        }
        
	}
}
