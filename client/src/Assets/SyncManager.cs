using UnityEngine;
using System.Collections;

public class SyncManager : Photon.MonoBehaviour {

    public float positionLerpSpeed = 0.1f;
    public float rotationLerpSpeed = 0.1f;
    public float teleportDistanceThreshold = 5;

    private Vector3 receivedPosition = Vector3.zero;
    private Quaternion receivedRotation = Quaternion.identity;
    private Animator animator;
    private SkillManager skillManager;
    private PlayerManager playerManager;
    private int receivedTeamId;
    private string receivedUsername;
    private float receivedHealth;
    private int receivedKills;
    private int receivedDeaths;
    private bool deathAnimationPlaying = false;
    private bool isDead = false;

    void Start()
    {
        animator = transform.GetComponent<Animator>();
        skillManager = transform.GetComponent<SkillManager>();
        playerManager = transform.GetComponent<PlayerManager>();
    }

	// Update is called once per frame
	void Update () {
	
        if (photonView.isMine)
        {
            // nothing to be done here at this stage
        } else
        {
            float distance = Vector3.Distance(transform.position, receivedPosition);
            if (distance < teleportDistanceThreshold)
            {
                transform.position = Vector3.Lerp(transform.position, receivedPosition, positionLerpSpeed);
            } else
            {
                transform.position = receivedPosition;
            }
            
            transform.rotation = Quaternion.Lerp(transform.rotation, receivedRotation, rotationLerpSpeed);
            playerManager.teamId = receivedTeamId;
            playerManager.username = receivedUsername;
            playerManager.health = receivedHealth;
            playerManager.kills = receivedKills;
            playerManager.deaths = receivedDeaths;
            playerManager.dead = isDead;
        }
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Debug.Log("syncing");
        if (stream.isWriting)
        {
            // rotation
            stream.SendNext(transform.position);
            // position
            stream.SendNext(transform.rotation);
            // animator.Speed
            stream.SendNext(animator.GetFloat("Speed"));
            // animator.toTrigger
            //if (skillManager.enabled)
            //{
            //    stream.SendNext(skillManager.currentSkill);
            //    skillManager.currentSkill = "NOSKILL";
            //} else
            //{
            //    stream.SendNext("NOSKILL");
            //}
            // am I dead?
            stream.SendNext(playerManager.dead);
            // team id
            stream.SendNext(playerManager.teamId);
            // username
            stream.SendNext(playerManager.username);
            // health
            stream.SendNext(playerManager.health);
            // kills
            stream.SendNext(playerManager.kills);
            // deaths
            stream.SendNext(playerManager.deaths);
            
        }
        else {
            receivedPosition = (Vector3)stream.ReceiveNext();
            receivedRotation = (Quaternion)stream.ReceiveNext();
            float speed = (float)stream.ReceiveNext();
            animator.SetFloat("Speed", speed);
            //string skill = (string)stream.ReceiveNext();
            //if (!skill.Equals("NOSKILL"))
            //{
            //    animator.SetTrigger(skill);
            //}
            isDead = (bool)stream.ReceiveNext();
            if (isDead && !deathAnimationPlaying)
            {
                animator.SetTrigger("Death");
                deathAnimationPlaying = true;
            }
            if (!isDead && deathAnimationPlaying)
            {
                animator.SetTrigger("Respawn");
                deathAnimationPlaying = false;
            }
            receivedTeamId = (int)stream.ReceiveNext();
            receivedUsername = (string)stream.ReceiveNext();
            receivedHealth = (float)stream.ReceiveNext();
            receivedKills = (int)stream.ReceiveNext();
            receivedDeaths = (int)stream.ReceiveNext();
        }
    }
}
