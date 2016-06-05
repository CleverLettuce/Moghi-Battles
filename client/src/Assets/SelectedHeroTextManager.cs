using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectedHeroTextManager : MonoBehaviour {

    public Text text;
	
	// Update is called once per frame
	void Update () {

        text.text = "Selected hero: " + HeroSelector.heroName;

    }
}
