using UnityEngine;
using System.Collections;
using System;

public class BMBasicAttack : Skill
{
    public float damageDelay;
    private bool damageDealt = false;
    private PlayerManager myManager;
    private float damageTime;

    void Start()
    {
        base.Start();

        skillName = "BasicAttack";
        myManager = transform.GetComponent<PlayerManager>();
        damageTime = damageDelay * skillDuration;
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
        if (skillElapsedTime > damageTime && !damageDealt)
        {
            damageDealt = true;
            doDamage();
        }
    }

    protected override void handleSkillFired()
    {
        Debug.Log("Skill BMBasicAttack activated");
        animator.SetTrigger(skillName);
        damageDealt = false;
    }

    protected override void handleSkillEnded()
    {
    }
}
