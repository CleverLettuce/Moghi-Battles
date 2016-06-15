using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectedMapTextManager : MonoBehaviour {

    public Text text;
	
	// Update is called once per frame
	void Update () {

        text.text = "Slected map: " + Util.decodeMapName(MapSelectionManager.mapName);
	}
}
