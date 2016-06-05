using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillCDIconManager : MonoBehaviour {

    public GameObject textObject;
    private Text text;
    public Image image;
    public Skill skill;

    void Start()
    {
        text = textObject.GetComponent<Text>();
    }

	public void dim()
    {
        image.color = new Color(0.39f, 0.39f, 0.39f);
    }

    public void brighten()
    {
        image.color = new Color(1, 1, 1);
    }

    public void setText(string text)
    {
        if (this.text == null)
        {
            return;
        }
        this.text.text = text;
    }

    public void setSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
