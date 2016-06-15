using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PickUpManager : Photon.MonoBehaviour {

    public KeyCode pickUpKey = KeyCode.F;
    public float pickUpDistance = 2f;
    public GameObject pickUpItem;
    public GameObject infoTextObject;
    private Text text;
    private TriggerManager triggerManager;
    private bool pickedUp;
    public float spawnChance = 1;
    public AudioClip pickUpSound;

    void Start()
    {
        infoTextObject = GameObject.Find("InfoText");
        text = infoTextObject.GetComponent<Text>();
        triggerManager = transform.GetComponent<TriggerManager>();
    }

	void Update () {

        if (pickedUp)
        {
            if (PhotonNetwork.player.isMasterClient)
            {
                PhotonNetwork.Destroy(photonView);
            }
            return;
        }

        float distToPlayer = float.MaxValue;
        PlayerManager[] managers = FindObjectsOfType<PlayerManager>();
        PlayerManager me = null;
        foreach (PlayerManager player in managers)
        {
            if (player.username.Equals(PhotonNetwork.playerName))
            {
                distToPlayer = Vector3.Distance(transform.position, player.transform.position);
                me = player;
                break;
            }
        }
        if (distToPlayer < pickUpDistance)
        {
            if (Input.GetKeyUp(pickUpKey))
            {
                text.text = "";
                pickedUp = true;
                GameObject pickUpInstance = Instantiate(pickUpItem);
                pickUpInstance.transform.SetParent(me.transform);
                BuffManager bManager = pickUpInstance.GetComponent<BuffManager>();
                bManager.player = me;
                bManager.speedProvider = me.transform.GetComponent<EightDirMovement>();
                if (pickUpSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickUpSound, transform.position);
                }
                photonView.RPC("pickUpNotify", PhotonTargets.AllBuffered);
            }
        }
        
	}

    [PunRPC]
    public void pickUpNotify()
    {
        pickedUp = true;
    }
}
