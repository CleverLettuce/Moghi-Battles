using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackToLogin : MonoBehaviour {

	public void backToLogIn()
    {
        SceneManager.LoadScene("loginScene");
    }
}
