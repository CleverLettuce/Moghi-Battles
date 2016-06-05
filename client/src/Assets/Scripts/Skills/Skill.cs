using UnityEngine;
using System.Collections;
using System;

public abstract class Skill : MonoBehaviour, ISkill {

    public float cooldown;
    public float skillDuration;
    public float goldCost;
    public string description;
    public Sprite skillIcon;

    public GameObject skillTrigger;
    public float triggerDistanceFromPlayer;
    public float triggerAngleFromPlayer;
    public Vector3 triggerRotation;
    public string skillName;

    protected float skillElapsedTime;
    protected float lastFired = -100000000.0f;
    protected Animator animator;
    protected bool skillFiring;
    protected TriggerManager triggerManager;
    protected Action onSkillCompletedAction;
    protected Action onSkillInterruptedAction;

    protected PhotonView pView;

    [Serializable]
    public class ParticleFX
    {
        public float startTime;
        public GameObject particles;
        public Vector3 positionRelativeToPlayer;
        public float destroyTime;
        [HideInInspector]
        public bool started = false;
    }

    public ParticleFX[] particleEffects;

    public void fire()
    {
        Debug.Log("Skill " + skillName + " activated");
        skillFiring = true;
        skillElapsedTime = 0.0f;
        lastFired = Time.time;
        animator.SetTrigger(skillName);
        foreach (ParticleFX particlefx in particleEffects)
        {
            particlefx.started = false;
        }

        if (triggerManager != null)
        {
            triggerManager.resetTrigger();
        }
        
        handleSkillFired();
    }

    void Update()
    {
        if (skillFiring)
        {
            if (skillElapsedTime > skillDuration)
            {
                skillFiring = false;
                if (onSkillCompletedAction != null)
                {
                    onSkillCompletedAction.Invoke();
                }
                if (triggerManager != null)
                {
                    triggerManager.resetTrigger();
                }
                
                handleSkillEnded();

                return;
            }

            skillElapsedTime += Time.deltaTime;
            handleSkillFiring();

            foreach (ParticleFX particlefx in particleEffects)
            {
                if (skillElapsedTime > particlefx.startTime * skillDuration && !particlefx.started)
                {
                    particlefx.started = true;
                    GameObject particleHolder = (GameObject)Instantiate(Resources.Load("EmptyObject"), transform.position, transform.rotation);
                    GameObject particleObj = (GameObject)Instantiate(particlefx.particles, transform.position, Quaternion.identity);
                    particleObj.transform.SetParent(particleHolder.transform);
                    particleObj.transform.localPosition = particlefx.positionRelativeToPlayer;
                    Destroy(particleHolder, particlefx.destroyTime);
                    IParticle particle = particleObj.GetComponent<IParticle>();
                    particle.run();
                }
            }
        }
    }

    public float getCooldown()
    {
        return cooldown;
    }

    public float getLastFired()
    {
        return lastFired;
    }

    protected void Start()
    {
        animator = transform.GetComponent<Animator>();
        Vector3 triggerSpawn = transform.position + Vector3.forward * triggerDistanceFromPlayer;
        if (skillTrigger != null)
        {
            GameObject trigger = ((GameObject)Instantiate(skillTrigger, triggerSpawn, Quaternion.identity));
            trigger.transform.parent = transform;
            trigger.transform.rotation = Quaternion.Euler(triggerRotation);
            triggerManager = trigger.GetComponent<TriggerManager>();
        }
        
        pView = transform.GetComponent<PhotonView>();
    }

    public string getName()
    {
        return skillName;
    }

    public void interruptSkill()
    {
        skillFiring = false;
        if (onSkillInterruptedAction != null)
        {
            onSkillInterruptedAction.Invoke();
        }
        
    }

    public void onSkillCompleted(Action action)
    {
        handleSkillEnded();
        onSkillCompletedAction = action;
    }

    public void onSkillInterrupted(Action action)
    {
        handleSkillEnded();
        onSkillInterruptedAction = action;
    }

    public bool isOnCD()
    {
        if (Time.time - lastFired < cooldown)
        {
            return true;
        } else
        {
            return false;
        }
    }

    protected abstract void handleSkillFiring();
    protected abstract void handleSkillFired();
    protected abstract void handleSkillEnded();

    
}
