using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : Photon.MonoBehaviour {

    public string VERSION = "alpha 0.0.1";
    public SpawnManager spawnManager;
    private bool loggedIn = false;
    private string token;
    private string username = "Username";
    private string password = "Password";
    private bool requireSpawn = false;
    private bool alreadyInRoom = false;
    private bool allTeamsFull = false;

	// Use this for initialization
	void Start () {
    }
	
    void Connect(string username, string password)
    {
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
        PhotonNetwork.AuthValues.AddAuthParameter("username", username);
        PhotonNetwork.AuthValues.AddAuthParameter("password", password);
        PhotonNetwork.ConnectUsingSettings(VERSION);
        this.username = username;
    }

    void OnJoinedLobby()
    {
        loggedIn = true;
        Debug.Log("Joined lobby");
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("No rooms found, creating room");
        PhotonNetwork.CreateRoom(null);
    }

    void OnJoinedRoom()
    {
        
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if (username.Equals(player.name))
            {
                PhotonNetwork.Disconnect();
                loggedIn = false;
                alreadyInRoom = true;
                return;
            }
        }
        alreadyInRoom = false;
        TeamManager teamManager = transform.GetComponent<TeamManager>();
        if (!teamManager.allTeamsFull())
        {
            requireSpawn = true;
            allTeamsFull = false;


        } else
        {
            PhotonNetwork.Disconnect();
            loggedIn = false;
            allTeamsFull = true;
            return;
        }
        
        PhotonNetwork.player.name = username;
    }

    // Update is called once per frame
    void Update () {
        if (requireSpawn)
        {
            requireSpawn = false;
            spawnManager.SpawnRandom(username);
        }
	}

    void OnGUI()
    {
        if (!loggedIn)
        {
            GUILayout.Label(PhotonNetwork.connectionState.ToString());
            username = GUILayout.TextField(username, 25);
            password = GUILayout.TextField(password, 25);
            if (GUILayout.Button("Log in"))
            {
                Debug.Log("Trying to connect with username: " + username + ", password: " + password + ".");
                Connect(username, password);
            }
        } else
        {
            GUILayout.Label("Currently logged in as " + username + ".");
        }
        
        if (alreadyInRoom)
        {
            GUILayout.Label("You're already logged in this room");
        }
        if (allTeamsFull)
        {
            GUILayout.Label("This room is full!");
        }
    }

    void OnCustomAuthenticationFailed()
    {
        Debug.Log("Failed log in");
    }
}
