using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour {

    public string VERSION = "alpha 0.1";
    private static bool loggedIn = false;
    private static string token;
    private static string username;
    private static string password;
    private Text errorText;
    private InputField usernameInput;
    private InputField passwordInput;
    private float loginAttemptStart;

    void Start()
    {
        GameObject textObject = GameObject.Find("LoginErrorText");
        errorText = textObject.GetComponent<Text>();
        Assert.IsNotNull<Text>(errorText);
        GameObject usernameObject = GameObject.Find("UsernameInputField");
        Assert.IsNotNull<GameObject>(usernameObject);
        usernameInput = usernameObject.GetComponent<InputField>();

        GameObject passwordObject = GameObject.Find("PasswordInputField");
        passwordInput = passwordObject.GetComponent<InputField>();
    }


    public void TryConnect()
    {
        errorText.text = "";
        Debug.Log("Attempting log in with username: " + usernameInput.text);
        loginAttemptStart = Time.time;
        Connect(usernameInput.text, passwordInput.text);
    }

    void Connect(string username, string password)
    {
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
        PhotonNetwork.AuthValues.AddAuthParameter("username", username);
        PhotonNetwork.AuthValues.AddAuthParameter("password", password);
        PhotonNetwork.ConnectUsingSettings(VERSION);
        LoginManager.username = username;
        LoginManager.password = password;
    }

    void OnJoinedLobby()
    {
        loggedIn = true;
        Debug.Log("Joined lobby");
        if (!LobbyManager.inLobbyScene)
        {
            SceneManager.LoadScene("lobbyScene");
        }
        
    }

    void OnCustomAuthenticationFailed()
    {
        if (errorText != null)
        {
            errorText.text = "Wrong username or password.";
        }
    }

    void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("debug msg: " + debugMessage);
        Debug.Log("Failed log in");
        if (errorText != null)
        {
            if (debugMessage.Contains("RequestTimeout"))
            {
                errorText.text = "Login request timeout.";
            } else
            {
                errorText.text = "Server encountered an error!";
            }
        }
    }

    void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        object value = null;
        data.TryGetValue("token", out value);
        if (value != null)
        {
            token = (string)value;
        }
    }

    public static string getUsername()
    {
        return username;
    }

    public static string getPassword()
    {
        return password;
    }

    public static string getToken()
    {
        return token;
    }

    public static bool isLoggedIn()
    {
        return loggedIn;
    }

    public static void logOut()
    {
        loggedIn = false;
        token = null;
        username = null;
        password = null;
    }

    public static void clear()
    {
        username = "";
        password = "";
        token = "";
    }
}
