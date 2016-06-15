using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LaddersManager : MonoBehaviour {

    [Serializable]
    public class LadderItem : IComparable<LadderItem>
    {
        public string user;
        public int score;

        public int CompareTo(LadderItem other)
        {
            int scoreComp = score.CompareTo(other.score);
            if (scoreComp != 0)
            {
                return -scoreComp;
            }

            return user.CompareTo(other.user);
        }
    }

    public class LaddersResponse
    {
        public int status;
        public LadderItem[] data;
    }

    public GameObject contentPanel;

	// Use this for initialization
	void Start () {

        StartCoroutine(fetchResults());
	}

    public IEnumerator fetchResults()
    {
        WWW www = new WWW(DataServerDomain.url + "scores");
        yield return www;
        LaddersResponse response = JsonUtility.FromJson<LaddersResponse>(www.text);
        if (response.status == 200)
        {
            for (int i = 0, childCount = contentPanel.transform.childCount; i < childCount; i++)
            {
                Transform child = contentPanel.transform.GetChild(i);
                Destroy(child.gameObject);
            }
            Array.Sort(response.data);
            foreach (LadderItem item in response.data)
            {
                GameObject nameText = (GameObject)Instantiate(Resources.Load("TableText", typeof(GameObject)));
                GameObject resultText = (GameObject)Instantiate(Resources.Load("TableText", typeof(GameObject)));

                nameText.GetComponent<Text>().text = item.user;
                resultText.GetComponent<Text>().text = item.score.ToString();

                nameText.transform.SetParent(contentPanel.transform);
                resultText.transform.SetParent(contentPanel.transform);
            }
        }
    }

    public void refresh()
    {
        StartCoroutine(fetchResults());
    }

    public void backToLobby()
    {
        SceneManager.LoadScene("lobbyScene");
    }

}
