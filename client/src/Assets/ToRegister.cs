using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ToRegister : MonoBehaviour {

	public void toRegister()
    {
        SceneManager.LoadScene("registerScene");
    }
}
