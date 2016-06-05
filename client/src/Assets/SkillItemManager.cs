using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillItemManager : MonoBehaviour {
    public Image image;

    public void setSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
