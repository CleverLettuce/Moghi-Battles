using UnityEngine;
using System.Collections;
using System;

public class PlayerManager : MonoBehaviour, IDamagable {

    public float attack;
    public float defense;
    public float gold = 300;
    public float goldRecoverSpeed = 1;
    public float goldRecoverAmount = 1;

    public int teamId;
    public int kills;
    public int deaths;

    public float respawnTime;

    public float maxHealth;
    public float health;
    public string username;
    public string heroName;

    public bool dead = false;
    public bool activateGoldTickUp;
    public PhotonView view;
    public Animator animator;
    public SkillManager skillManager;
    public GameObject[] myTeamSpawns;

    public GameObject teamMarker;

	// Use this for initialization
	void Start () {
        health = maxHealth;
        view = transform.GetComponent<PhotonView>();
        animator = transform.GetComponent<Animator>();
        skillManager = transform.GetComponent<SkillManager>();
        StartCoroutine(tickGoldUp());
	}

    private IEnumerator tickGoldUp()
    {
        while (true)
        {
            Debug.Log("Gold recover activated");
            yield return new WaitForSeconds(goldRecoverSpeed);
            if (activateGoldTickUp)
            {
                gold += goldRecoverAmount;
            }
            
        }
        
    }

    private float calcDamage(float opponentAttack)
    {
        float damage = 10 * opponentAttack / defense * UnityEngine.Random.Range(0.75f, 1.25f);
        return damage;
    }

    [PunRPC]
    public void takeDamage(float opponentAttack, string playerName)
    {
        if (dead)
        {
            return;
        }
        float damage = calcDamage(opponentAttack);
        health -= damage;
        if (health <= 0)
        {
            dead = true;
            deaths++;
            Debug.Log("Player " + playerName + " killed " + username);
            if (view.isMine)
            {
                try
                {
                    skillManager.interruptCurrentSkill();
                } catch (Exception e)
                {
                    Debug.LogError("Couldn't interrupt skill: " + e.Message);
                }
                
                processKill(playerName);
            }

        }
    }

    [PunRPC]
    public void incKills()
    {
        kills++;
    }

    [PunRPC]
    public void rewardGold(float goldBonus = 10)
    {
        gold += goldBonus;
    }

    void processKill(string playerName)
    {
        Debug.Log("processing kill");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            if (playerManager == null)
            {
                Debug.LogError("Player without PlayerManager found!");
                return;
            } else
            {
                if (playerManager.username.Equals(playerName))
                {
                    playerManager.view.RPC("incKills", PhotonTargets.All, new object[] { });
                    playerManager.view.RPC("rewardGold", PhotonTargets.All, 10);
                    break;
                }

            }
        }
        Debug.Log("respawning");
        StartCoroutine(respawn());

    }

    GameObject selectRespawnSpot()
    {
        int selector = UnityEngine.Random.Range(0, myTeamSpawns.Length);
        return myTeamSpawns[selector];
    }

    IEnumerator respawn()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(respawnTime);
        GameObject respawnSpot = selectRespawnSpot();
        transform.position = new Vector3(respawnSpot.transform.position.x, respawnSpot.transform.position.y, respawnSpot.transform.position.z);
        animator.SetTrigger("Respawn");
        health = maxHealth;
        dead = false;
    }

    public PhotonView getView()
    {
        return view;
    }

    public int getTeamId()
    {
        return teamId;
    }

    public bool isDead()
    {
        return dead;
    }

    public float getHealth()
    {
        return health;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public void setTeamMarker(GameObject marker)
    {
        if (teamMarker != null)
        {
            teamMarker.SetActive(false);
            Destroy(teamMarker);
        }

        teamMarker = marker;
        teamMarker.transform.SetParent(transform);
        teamMarker.transform.position += new Vector3(0, 0.05f, 0);
    }

    public GameObject getTeamMarker()
    {
        return teamMarker;
    }
}
