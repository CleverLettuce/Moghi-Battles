using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeroSelectionManager : MonoBehaviour {

    public GameObject heroNamePanel;
    public GameObject attackDesc;
    public GameObject defenseDesc;
    public GameObject speedDesc;
    public GameObject playStyleDesc;
    public GameObject skillInfoPanel;

    private Text heroNameText;
    private Text attackDescText;
    private Text defenseDescText;
    private Text speedDescText;
    private Text playStyleDescText;

    public GameObject[] availableHeroes;
    public int selector = 0;

    private GameObject skillItemPrototype;

    // Use this for initialization
    void Start () {

        availableHeroes = new GameObject[2];
        availableHeroes[0] = (GameObject)Resources.Load("Barbarian mage/Barbarian mage");
        availableHeroes[1] = (GameObject)Resources.Load("Barbarian Shaman/witch doctor");

        heroNameText = heroNamePanel.GetComponent<Text>();
        attackDescText = attackDesc.GetComponent<Text>();
        defenseDescText = defenseDesc.GetComponent<Text>();
        speedDescText = speedDesc.GetComponent<Text>();
        heroNameText = heroNamePanel.GetComponent<Text>();
        playStyleDescText = playStyleDesc.GetComponent<Text>();
        skillItemPrototype = (GameObject)Resources.Load("SkillInfoItem");
    }
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(refresh());
    }

    public void next()
    {
        selector = (selector + 1) % availableHeroes.Length;
    }

    public void select()
    {
        switch (selector)
        {
            case 0: HeroSelector.hero = "Barbarian mage/Barbarian mage";
                HeroSelector.heroName = "La'uk";  break;
            case 1: HeroSelector.hero = "Barbarian Shaman/witch doctor";
                HeroSelector.heroName = "Eeyo"; break;
        }

        SceneManager.LoadScene("lobbyScene");
    }

    public IEnumerator refresh()
    {
        yield return new WaitForSeconds(0.4f);
        heroNameText.text = availableHeroes[selector].GetComponent<PlayerManager>().heroName;
        Description desc = availableHeroes[selector].GetComponent<Description>();
        attackDescText.text = "Attack: " + desc.attackRating;
        defenseDescText.text = "Defense: " + desc.defenseRating;
        speedDescText.text = "Speed: " + desc.speedRanking;
        playStyleDescText.text = "Play style: " + desc.playStyle;

        for (int i = 0; i < skillInfoPanel.transform.childCount; i++)
        {
            GameObject obj = skillInfoPanel.transform.GetChild(i).gameObject;
            Destroy(obj);
        }

        Skill[] skills = availableHeroes[selector].GetComponents<Skill>();
        foreach (Skill skill in skills)
        {
            GameObject skillItem = Instantiate(skillItemPrototype);
            Text skillDesc = skillItem.GetComponentInChildren<Text>();
            SkillItemManager man = skillItem.GetComponentInChildren<SkillItemManager>();
            man.setSprite(skill.skillIcon);
            skillDesc.text = skill.description;

            skillItem.transform.SetParent(skillInfoPanel.transform);
        }
    }


}
