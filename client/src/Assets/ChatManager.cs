using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChatManager : Photon.MonoBehaviour {

    public GameObject canvasContent;
    public GameObject textFieldObject;
    public int maxCharsPerMessage = 64;
    public bool writePhase;

	void Update () {
	
        if (Input.GetKeyUp(KeyCode.Return))
        {
            SkillManager skillManager = FindObjectOfType<SkillManager>();
            EightDirMovement movement = FindObjectOfType<EightDirMovement>();
            InputField field = textFieldObject.GetComponent<InputField>();
            if (!writePhase)
            {
                writePhase = true;
                
                skillManager.disableInput = true;
                
                movement.disableInput = true;
                field.ActivateInputField();
                textFieldObject.GetComponent<InputField>().Select();
                return;
            }

            writePhase = false;
            skillManager.disableInput = false;
            movement.disableInput = false;
            field.DeactivateInputField();
            PlayerManager[] players = FindObjectsOfType<PlayerManager>();
            foreach (PlayerManager player in players)
            {
                if (player.view != null && player.view.isMine)
                {
                    photonView.RPC("sendMessage", PhotonTargets.All, field.text, player.username, player.teamId);
                }
            }
            
        }
	}
    
    [PunRPC]
    void sendMessage(string message, string senderName, int teamId)
    {
        PlayerManager[] players = FindObjectsOfType<PlayerManager>();
        foreach (PlayerManager player in players)
        {
            if (player.view != null && player.view.isMine)
            {
                if (teamId != player.teamId)
                {
                    return;
                } else
                {
                    break;
                }
            }
        }
        if (message.Length > maxCharsPerMessage)
        {
            message = message.Substring(0, maxCharsPerMessage);
        }


        GameObject textObj = (GameObject)Instantiate(Resources.Load("ChatText"));
        textObj.GetComponent<Text>().text = senderName + ": " + message;
        textObj.transform.SetParent(canvasContent.transform);

    }
}
