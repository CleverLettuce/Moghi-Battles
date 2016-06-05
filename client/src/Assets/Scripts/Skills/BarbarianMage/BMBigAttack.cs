using UnityEngine;
using System.Collections;
using System;

public class BMBigAttack : Skill {

    public float firstDamageDelay;
    private bool firstDamageDealt = false;
    public float secondDamageDelay;
    private bool secondDamageDealt = false;
    private PlayerManager myManager;
    private float firstDamageTime;
    private float secondDamageTime;

    void Start()
    {
        base.Start();

        skillName = "BasicAttack";
        myManager = transform.GetComponent<PlayerManager>();
    }

    private void doDamage()
    {
        Debug.Log("DoDamageCalled");
        IDamagable[] objects = triggerManager.getObjectsInTrigger();
        foreach (IDamagable obj in objects)
        {
            if (obj.getTeamId() == myManager.teamId)
            {
                continue;
            }
            Debug.Log("Opponent found");
            PhotonView view = obj.getView();
            if (view == null)
            {
                Debug.LogError("Player without photon view found!");
            }
            else
            {
                if (!obj.isDead())
                {
                    view.RPC("takeDamage", PhotonTargets.All, myManager.attack, myManager.username);
                }
            }
        }

    }

    protected override void handleSkillFiring()
    {
    }

    protected override void handleSkillFired()
    {
        Debug.Log("Skill BMBasicAttack activated");
        animator.SetTrigger(skillName);
    }

    protected override void handleSkillEnded()
    {
    }
}
