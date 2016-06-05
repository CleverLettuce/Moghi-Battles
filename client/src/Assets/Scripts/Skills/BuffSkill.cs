using UnityEngine;
using System.Collections;
using System;

public class BuffSkill : Skill {

    public GameObject buff;
    public float buffDuration;

    protected override void handleSkillEnded()
    {}

    protected override void handleSkillFired()
    {
        GameObject buffObj = (GameObject)Instantiate(buff, transform.position, transform.rotation);
        buffObj.transform.SetParent(transform);
        
        BuffManager manager = buffObj.GetComponent<BuffManager>();
        manager.player = transform.GetComponent<PlayerManager>();
        manager.speedProvider = transform.GetComponent<ISpeedProvider>();
        Destroy(buffObj, buffDuration);
    }

    protected override void handleSkillFiring()
    {}
}
