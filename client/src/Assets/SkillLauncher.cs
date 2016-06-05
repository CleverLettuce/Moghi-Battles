using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillLauncher : Photon.MonoBehaviour {

    Dictionary<string, Skill> skills = new Dictionary<string, Skill>();
    public bool waitingForServerResponse = false;

    void Start()
    {
        Skill[] mySkills = transform.GetComponents<Skill>();
        foreach (Skill skill in mySkills)
        {
            skills.Add(skill.skillName, skill);
        }
    }

    [PunRPC]
	public void launchSkill(string skillName)
    {
        Skill skill = skills[skillName];
        skill.fire();
        waitingForServerResponse = false;
    }
}
