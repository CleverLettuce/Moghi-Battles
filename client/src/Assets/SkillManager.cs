using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SkillManager : MonoBehaviour {

    private Animator animator;
    public bool disableInput;

    [Serializable]
    public class SkillActivation
    {
        public KeyCode keyCode;
        public Skill skill;
    }

    public SkillActivation[] skills;

    private Boolean skillRuning = false;
    public string currentSkill = "NOSKILL";
    public SkillActivation currentSkillActivation = null;
    public PlayerManager player;
    private PhotonView view;
    private SkillLauncher skillLauncher;

	// Use this for initialization
	void Start () {
        animator = transform.GetComponent<Animator>();
        Action resetCurrentSkill = () =>
        {
            currentSkill = "NOSKILL";
            currentSkillActivation = null;
        };
        foreach (SkillActivation activation in skills)
        {
            activation.skill.onSkillCompleted(resetCurrentSkill);
            activation.skill.onSkillInterrupted(resetCurrentSkill);
        }
        view = transform.GetComponent<PhotonView>();
        skillLauncher = transform.GetComponent<SkillLauncher>();
        player = transform.GetComponent<PlayerManager>();
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("animator state: " + animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
        if (skillLauncher.waitingForServerResponse)
        {
            return;
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("free") && !animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            return;
        }
        if (disableInput)
        {
            return;
        }
        foreach (SkillActivation activation in skills)
        {
            if (!Input.GetKey(activation.keyCode))
            {
                continue;
            }

            //Debug.Log("Activation: " + activation.keyCode);

            if (activation.skill.getLastFired() +  activation.skill.getCooldown() > Time.time || player.gold < activation.skill.goldCost)
            {
                return;
            }

            player.gold -= activation.skill.goldCost;
            currentSkill = activation.skill.getName();
            currentSkillActivation = activation;
            skillLauncher.waitingForServerResponse = true;
            skillLauncher.photonView.RPC("launchSkill", PhotonTargets.AllViaServer, currentSkill);

            //activation.skill.fire();
            return;
        }
	}

    public void interruptCurrentSkill()
    {
        if (currentSkillActivation != null)
        {
            currentSkillActivation.skill.interruptSkill();
        }
    }
}
