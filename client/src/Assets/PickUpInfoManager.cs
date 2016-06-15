using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PickUpInfoManager : MonoBehaviour {

    public GameObject pickUpInfoText;
    public Text text;

	// Use this for initialization
	void Start () {
        text = pickUpInfoText.GetComponent<Text>();
	}

    // Update is called once per frame
    void Update()
    {
        PlayerManager me = null;
        PlayerManager[] players = FindObjectsOfType<PlayerManager>();
        foreach (PlayerManager player in players)
        {
            if (player.view.isMine)
            {
                me = player;
            }
        }

        if (me == null)
        {
            return;
        }
        PickUpManager[] pickUps = FindObjectsOfType<PickUpManager>();
        bool inRange = false;
        foreach (PickUpManager pickUp in pickUps)
        {
            if (Vector3.Distance(pickUp.transform.position, me.transform.position) < pickUp.pickUpDistance)
            {
                inRange = true;
                break;
            }
        }

        if (inRange)
        {
            text.text = "Press F to pick this up.";
        } else
        {
            text.text = "";
        }
    }
}
