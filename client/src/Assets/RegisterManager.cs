using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour {

    public GameObject usernameField;
    public GameObject passwordField;
    public GameObject emailField;
    public GameObject statusText;

    public class RegisterResponse
    {
        public int status;
    }

    public void register()
    {
        StartCoroutine(doRegister());
    }

    public IEnumerator doRegister()
    {
        Text text = statusText.GetComponent<Text>();
        text.text = "";
        
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.GetComponent<InputField>().text);
        form.AddField("password", passwordField.GetComponent<InputField>().text);
        form.AddField("email", emailField.GetComponent<InputField>().text);
        WWW reg = new WWW(DataServerDomain.url + "user", form.data);
        yield return reg;
        RegisterResponse response = JsonUtility.FromJson<RegisterResponse>(reg.text);
        
        if (response.status == 200)
        {
            text.color = Color.green;
        } else
        {
            text.color = Color.red;
        }

        switch (response.status)
        {
            case 200: text.text = "Successful registration"; break;
            case 4000: text.text = "Username must consist of 3 or more numbers and/or letters."; break;
            case 4001: text.text = "Password must be at least 8 characters long."; break;
            case 4002: text.text = "Invalid e-mail address."; break;
            case 4003: text.text = "Username already exists."; break;
            case 4004: text.text = "Another user is registered with this e-mail address."; break;
            default: text.text = "Unknown error."; break;
        }

    }

}
