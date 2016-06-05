using UnityEngine;
using System.Collections;
using System;

public class DamageSkill : Skill {

    protected PlayerManager myManager;
    public DamageEffect[] damageEffects;

    [Serializable]
    public class DamageEffect
    {
        public float damageDelay;
        public float damageModifier;
        [HideInInspector]
        public bool damageDealt = false;
    }

    // Use this for initialization
    void Start () {
        base.Start();

        myManager = transform.GetComponent<PlayerManager>();
        
    }

    protected override void handleSkillFiring()
    {
        if (!pView.isMine)
        {
            return;
        }

        //base.handleSkillFiring();
        foreach (DamageEffect effect in damageEffects)
        {
            //Debug.Log("Damage delay: " + effect.damageDelay + " Skill duration: " + skillDuration);
            if (skillElapsedTime > effect.damageDelay * skillDuration && !effect.damageDealt)
            {
                effect.damageDealt = true;
                doDamage(effect);
            }
        }
        
    }

    protected void doDamage(DamageEffect effect)
    {
        
        //Debug.Log("DoDamageCalled");
        if (triggerManager == null)
        {
            return;
        }
        IDamagable[] objects = triggerManager.getObjectsInTrigger();
        foreach (IDamagable obj in objects)
        {
            if (obj.getTeamId() == myManager.teamId)
            {
                continue;
            }
            //Debug.Log("Opponent found");
            PhotonView view = obj.getView();
            if (view == null)
            {
                Debug.LogError("Player without photon view found!");
            }
            else
            {
                if (!obj.isDead())
                {
                    view.RPC("takeDamage", PhotonTargets.All, myManager.attack * effect.damageModifier, myManager.username);
                }
            }
        }
    }

    protected override void handleSkillFired()
    {

        animator.SetTrigger(skillName);
        foreach (DamageEffect effect in damageEffects)
        {
            effect.damageDealt = false;
        }

        if (triggerManager != null)
        {
            triggerManager.transform.gameObject.SetActive(true);
        }
        
    }

    protected override void handleSkillEnded()
    {
        if (triggerManager != null)
        {
            triggerManager.transform.gameObject.SetActive(false);
        }
        
    }
}
