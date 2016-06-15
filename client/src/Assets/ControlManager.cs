using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControlManager : MonoBehaviour {

    public void exitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void returnToLobby()
    {
        PhotonNetwork.LeaveRoom();
        
    }

    void OnLeftRoom()
    {
        PhotonNetwork.player.name = "_1_abc";
        // (╯°□°）╯︵ ┻━┻ (ノಠ益ಠ)ノ彡┻━┻ ༼ つ ◕_◕ ༽つAMENO༼ つ ◕_◕ ༽つ
        if (!LobbyManager.inLobbyScene)
        {
            PhotonNetwork.LoadLevel("lobbyScene");
        }
        
    }

    public void logOff()
    {
        PhotonNetwork.Disconnect();
        LoginManager.clear();
    }

    void OnDisconnectedFromPhoton()
    {
        if (!SceneManager.GetActiveScene().name.Equals("loginScene")){
            LobbyManager.inLobbyScene = false;
            SceneManager.LoadScene("loginScene");
        }
        
    }
}
