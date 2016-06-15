using UnityEngine;
using System.Collections;

public class WallManager : Photon.MonoBehaviour, IDamagable {

    public int teamId;

    public float maxHealth;
    public float health;

    public bool dead = false;
    public float fallSpeed;
    public PhotonView view;
    public TeamManager teamManager;

    private Vector3 deadPos = Vector3.zero;
    private Vector3 receivedPosition;
    private int receivedTeamId;
    private float receivedHealth;
    public float positionLerpSpeed;
    public float goldReward = 10;
    public AudioClip takeDamageSound;

    void Start()
    {
        teamManager = FindObjectOfType<TeamManager>();
        view = transform.GetComponent<PhotonView>();
        teamId = teamManager.blueTeamId;
        health = maxHealth;
        receivedHealth = health;
        receivedPosition = transform.localPosition;
    }

    void Update()
    {
        if (!PhotonNetwork.inRoom)
        {
            return;
        }
        if (photonView.isMine)
        {
            if (dead)
            {
                if (deadPos.Equals(Vector3.zero))
                {
                    deadPos = new Vector3(transform.localPosition.x, transform.localPosition.y - 10, transform.localPosition.z);
                }

                transform.localPosition = Vector3.Lerp(transform.localPosition, deadPos, fallSpeed / 1000);
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, receivedPosition, positionLerpSpeed);
            teamId = receivedTeamId;
            health = receivedHealth;
        }
        
    }

    public int getTeamId()
    {
        return teamId;
    }

    public bool isDead()
    {
        return dead;
    }

    [PunRPC]
    public void takeDamage(float opponentAttack, string playerName)
    {
        Debug.Log("TakeDamageOnWallCalled");
        if (dead)
        {
            return;
        }
        if (takeDamageSound != null)
        {
            AudioSource.PlayClipAtPoint(takeDamageSound, transform.position);
        }

        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }
        health -= opponentAttack;
        if (health <= 0)
        {
            dead = true;
            PlayerManager[] players = FindObjectsOfType<PlayerManager>();
            foreach (PlayerManager player in players)
            {
                if (player.username.Equals(playerName))
                {
                    player.view.RPC("rewardGold", PhotonTargets.AllViaServer, goldReward);
                }
            }
        }
    }

    public PhotonView getView()
    {
        return view;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (!PhotonNetwork.inRoom)
        {
            return;
        }
        if (stream.isWriting)
        {
            // rotation
            stream.SendNext(transform.localPosition);
            // am I dead?
            stream.SendNext(dead);
            // team id
            stream.SendNext(teamId);
            // health
            stream.SendNext(health);

        }
        else {
            receivedPosition = (Vector3)stream.ReceiveNext();
            dead = (bool)stream.ReceiveNext();
            receivedTeamId = (int)stream.ReceiveNext();
            receivedHealth = (float)stream.ReceiveNext();
        }
    } 

    public float getHealth()
    {
        return health;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }
}
