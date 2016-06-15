using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MapSelectionManager : MonoBehaviour {

    public bool state = true;
    public static string mapName = "sacredforest";

    public GameObject desertName;
    public GameObject desertDesc;
    public GameObject sacredName;
    public GameObject sacredDesc;

    public void toggle()
    {
        if (state)
        {
            state = false;
            mapName = "desert";
            sacredName.SetActive(false);
            sacredDesc.SetActive(false);
            desertName.SetActive(true);
            desertDesc.SetActive(true);
        } else
        {
            state = true;
            mapName = "sacredForest";
            sacredName.SetActive(true);
            sacredDesc.SetActive(true);
            desertName.SetActive(false);
            desertDesc.SetActive(false);
        }
    }

    public void openLobby()
    {
        SceneManager.LoadScene("lobbyScene");
    }
}
