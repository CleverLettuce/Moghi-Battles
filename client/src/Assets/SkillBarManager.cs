using UnityEngine;
using System.Collections;
using System;

public class SkillBarManager : Photon.MonoBehaviour {

    public GameObject skillBar;
    public GameObject player;
    private PlayerManager playerManager;
    private bool initialized = false;
    public GameObject goldCounterTextObject;

    void Start()
    {
        StartCoroutine(updateSkillsInSKillBar());
    }
	
    private IEnumerator updateSkillsInSKillBar()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            if (!initialized)
            {
                PlayerManager[] players = FindObjectsOfType<PlayerManager>();
                foreach (PlayerManager player in players)
                {
                    PhotonView view = player.getView();
                    if (view.isMine)
                    {
                        this.player = player.transform.gameObject;
                        playerManager = player;
                        goldCounterTextObject.GetComponent<GoldCounterManager>().playerManager = player;
                    }
                }
                if (this.player == null)
                {
                    continue;
                }

                GameObject skillCDIconPrototype = (GameObject)Resources.Load("SkillCDIcon");

                Skill[] skills = this.player.GetComponents<Skill>();
                foreach (Skill skill in skills)
                {
                    GameObject skillCDIcon = Instantiate(skillCDIconPrototype);
                    skillCDIcon.transform.SetParent(skillBar.transform);
                    SkillCDIconManager iconManager = skillCDIcon.GetComponent<SkillCDIconManager>();
                    iconManager.skill = skill;
                    iconManager.setSprite(skill.skillIcon);
                }

                initialized = true;
            }
           
            SkillCDIconManager[] skillCDIcons = skillBar.GetComponentsInChildren<SkillCDIconManager>();
            foreach (SkillCDIconManager skillCDIcon in skillCDIcons)
            {
                Skill skill = skillCDIcon.skill;
                if (skill.isOnCD() || playerManager.gold < skill.goldCost)
                {
                    if (skill.isOnCD())
                    {
                        float elapsedTime = Time.time - skill.getLastFired();
                        int cdCount = Mathf.RoundToInt(skill.cooldown - elapsedTime);
                        skillCDIcon.dim();
                        skillCDIcon.setText("" + cdCount);
                    } else
                    {
                        skillCDIcon.dim();
                        skillCDIcon.setText("G");
                    }
                    
                }
                else
                {
                    skillCDIcon.brighten();
                    skillCDIcon.setText("");
                }
            }

            
        }
        
    }
}
