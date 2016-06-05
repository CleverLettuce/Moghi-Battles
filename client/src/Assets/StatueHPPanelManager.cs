using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatueHPPanelManager : MonoBehaviour {

    public GameObject statueText;
    public GameObject statue;
    private Text text;
    private IDamagable statueDamagable;
    private Gradient grad;

	// Use this for initialization
	void Start () {
        
        grad = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[4];
        colorKeys[0].color = Color.white;
        colorKeys[0].time = 0.0f;
        colorKeys[1].color = Color.green;
        colorKeys[1].time = 0.33f;
        colorKeys[2].color = Color.yellow;
        colorKeys[2].time = 0.67f;
        colorKeys[3].color = Color.red;
        colorKeys[3].time = 1.0F;


        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[4];
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 0.33f;
        alphaKeys[2].alpha = 1.0f;
        alphaKeys[2].time = 0.67f;
        alphaKeys[3].alpha = 1.0f;
        alphaKeys[3].time = 1.0f;

        grad.SetKeys(colorKeys, alphaKeys);


        text = statueText.GetComponent<Text>();
        statueDamagable = statue.GetComponent<IDamagable>();
	}
	
	// Update is called once per frame
	void Update () {

        float health = statueDamagable.getHealth();
        float maxHealth = statueDamagable.getMaxHealth();

        text.text = Mathf.Ceil(health).ToString();
        text.color = grad.Evaluate(1 - health / maxHealth);

	}
}
