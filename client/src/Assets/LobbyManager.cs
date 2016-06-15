using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviour {

    public GameObject tableContent;
    public GameObject errorPanel;
    public GameObject errorTextObject;
    private Text errorText;
    public static bool inLobbyScene;
    public bool gameFailed = false;
    public static int gameId = -1;

    public class GameResponse
    {
        public int status;
        public int data;
    }

	// Use this for initialization
	void Start () {
        gameId = -1;
        inLobbyScene = true;
        errorText = errorTextObject.GetComponent<Text>();
        errorPanel.SetActive(false);
        loadRooms();
        //PhotonNetwork.JoinRandomRoom();
    }

    public void buttonRefresh()
    {
        errorPanel.SetActive(false);
        loadRooms();
    }

    public void loadRooms()
    {
        for (int i = 0, childCount = tableContent.transform.childCount; i < childCount; i++)
        {
            Transform child = tableContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
        foreach (RoomInfo room in PhotonNetwork.GetRoomList())
        {
            if (room.visible)
            {
                loadRoom(room);
            } else
            {
                Debug.Log("Room not visible");
            }
            
        }
    }

    private void loadRoom(RoomInfo room)
    {
        Debug.Log("loading room");
        GameObject nameText = (GameObject)Instantiate(Resources.Load("TableText", typeof(GameObject)));
        GameObject mapNameText = (GameObject)Instantiate(Resources.Load("TableText", typeof(GameObject)));
        //GameObject dateCreatedText = (GameObject)Instantiate(Resources.Load("TableText", typeof(GameObject)));
        GameObject numberOfPlayersText = (GameObject)Instantiate(Resources.Load("TableText", typeof(GameObject)));
        GameObject joinButton = (GameObject)Instantiate(Resources.Load("JoinButton", typeof(GameObject)));
        
        object extractor;
        string roomName = room.name;
        string mapName;
        if (room.customProperties.TryGetValue("mapName", out extractor))
        {
            mapName = (string)extractor;
        }
        else
        {
            mapName = "No name";
        }

        //string dateCreated;
        //if (room.customProperties.TryGetValue("dateCreated", out extractor))
        //{
        //    dateCreated = (string)extractor;
        //}
        //else
        //{
        //    dateCreated = "Unknown";
        //}

        string numberOfPlayers = "" + room.playerCount + "/" + room.maxPlayers;

        nameText.GetComponent<Text>().text = roomName;
        mapNameText.GetComponent<Text>().text = Util.decodeMapName(mapName);
        //dateCreatedText.GetComponent<Text>().text = dateCreated;
        numberOfPlayersText.GetComponent<Text>().text = numberOfPlayers;
        if (room.playerCount == room.maxPlayers)
        {
            numberOfPlayersText.GetComponent<Text>().color = new Color(200, 0, 0);
        }

        nameText.transform.SetParent(tableContent.transform);
        mapNameText.transform.SetParent(tableContent.transform);
        //dateCreatedText.transform.SetParent(tableContent.transform);
        numberOfPlayersText.transform.SetParent(tableContent.transform);

        joinButton.GetComponent<ButtonContext>().roomName = roomName;
        Button.ButtonClickedEvent clickEvent = new Button.ButtonClickedEvent();
        clickEvent.AddListener(() => joinRoom(roomName));
        joinButton.GetComponent<Button>().onClick = clickEvent;
        joinButton.transform.SetParent(tableContent.transform);
    }

    void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        errorPanel.SetActive(true);
        errorText.text = codeAndMsg[1].ToString();
        Debug.LogError(codeAndMsg[1]);
    }


    void OnJoinedRoom()
    {
        Debug.Log("joining room");
        Debug.Log("username: " + LoginManager.getUsername());
        Room room = PhotonNetwork.room;
        room.visible = true;
        object extractor;
        bool newlyCreated = false;

        string mapName;
        if (!room.customProperties.TryGetValue("mapName", out extractor))
        {
            ExitGames.Client.Photon.Hashtable mapNameProp = new ExitGames.Client.Photon.Hashtable();
            mapNameProp.Add("mapName", RoomCreator.mapName);
            mapName = RoomCreator.mapName;
        } else
        {
            mapName = (string)extractor;
        }

        string username = LoginManager.getUsername();
        PhotonPlayer[] playerlist = PhotonNetwork.playerList;
        Debug.Log(playerlist.ToString());
        foreach (PhotonPlayer player in playerlist)
        {
            if (username.Equals(player.name))
            {
                PhotonNetwork.LeaveRoom();
                
                return;
            }
        }

        StartCoroutine(publishOrJoinGame(mapName, !PhotonNetwork.room.customProperties.TryGetValue("gameId", out extractor)));

    }

    private IEnumerator publishOrJoinGame(string mapName, bool newlyCreated)
    {

        if(newlyCreated){
            WWWForm gameForm = new WWWForm();
            gameForm.AddField("token", LoginManager.getToken());
            gameForm.AddField("mapName", mapName);
            WWW game = new WWW(DataServerDomain.url + "create", gameForm.data);
            yield return game;
            GameResponse response = JsonUtility.FromJson<GameResponse>(game.text);
            if (response.status != 200)
            {
                Debug.Log(response.status);
                PhotonNetwork.Disconnect();
                yield return null;
            }
            else
            {
                gameFailed = false;
                ExitGames.Client.Photon.Hashtable gameIdTable = new ExitGames.Client.Photon.Hashtable();
                gameIdTable.Add("gameId", response.data);
                PhotonNetwork.room.SetCustomProperties(gameIdTable);
            }
        }

        PhotonNetwork.playerName = LoginManager.getUsername();
        inLobbyScene = false;
        PhotonNetwork.LoadLevel(mapName);

    }

    void OnLeftRoom()
    {
        errorPanel.SetActive(true);
        errorText.text = "You're already in this room";
        Debug.LogError("Player already in room.");
    }

    public void joinRoom(string name)
    {
        // (ノಠ益ಠ)ノ彡┻━┻ 
        // This static property needs to be reset before another attempt at creating room else the room will automatically find our player in it and falsly report that we've already joined it. 
        // Also, this static property can't be reset to "". This is not documented anywhere in the PUN API.
        PhotonNetwork.playerName = "_1_abc";
        ExitGames.Client.Photon.Hashtable tId = new ExitGames.Client.Photon.Hashtable();
        tId.Add("teamId", 0);
        PhotonNetwork.player.SetCustomProperties(tId);
        errorPanel.SetActive(false);
        Debug.Log("Joining room: " + name);
        PhotonNetwork.JoinRoom(name);
    }

    public void buttonJoinRoom(ButtonContext button)
    {
        joinRoom(button.roomName);
    }

    public void createRoom()
    {
        // (ノಠ益ಠ)ノ彡┻━┻ 
        // This static property needs to be reset before another attempt at creating room else the room will automatically find our player in it and falsly report that we've already joined it. 
        // Also, this static property can't be reset to "". This is not documented anywhere in the PUN API.
        PhotonNetwork.playerName = "_1_abc";
        ExitGames.Client.Photon.Hashtable tId = new ExitGames.Client.Photon.Hashtable();
        tId.Add("teamId", 0);
        PhotonNetwork.player.SetCustomProperties(tId);
        Debug.Log("player name: " + PhotonNetwork.player.name);
        RoomOptions options = new RoomOptions();
        ExitGames.Client.Photon.Hashtable initProps = new ExitGames.Client.Photon.Hashtable();
        switch (MapSelectionManager.mapName)
        {
            case "sacredforest": options.maxPlayers = 6;
                initProps.Add("mapName", "sacredforest");
                initProps.Add("players", new string[6] { "", "", "", "", "", "" });
                break;
            case "desert":
                options.maxPlayers = 6;
                initProps.Add("mapName", "desert");
                initProps.Add("players", new string[6] { "", "", "", "", "", "" });
                break;
        }

        options.customRoomProperties = initProps;
        options.customRoomPropertiesForLobby = new string[] { "mapName", "players" };
        string roomName = LoginManager.getUsername() + "'s game #" + Math.Abs(PhotonNetwork.ServerTimestamp.GetHashCode());
        PhotonNetwork.CreateRoom(roomName, options, PhotonNetwork.lobby);
    }

    public void openHeroSelection()
    {
        SceneManager.LoadScene("heroSelectionScene");
    }

    public void openMapSelection()
    {
        SceneManager.LoadScene("mapSelectionScene");
    }

    public void toLadders()
    {
        SceneManager.LoadScene("highscoreScene");
    }
}
