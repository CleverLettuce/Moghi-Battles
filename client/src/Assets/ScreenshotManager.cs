using UnityEngine;
using System.Collections;

public class ScreenshotManager : MonoBehaviour {

    public KeyCode screenshotKey = KeyCode.F10;

	// Update is called once per frame
	void Update () {
	
        if (Input.GetKeyUp(screenshotKey))
        {
            string filename = System.DateTime.Now.ToString() + "";
            filename = filename.Replace('/', '_');
            filename = filename.Replace(' ', '_');
            filename = filename.Replace(':', '_');
            Application.CaptureScreenshot("Screenshots/" + filename + ".png");
            Debug.Log("Screenshot saved to: " + Application.persistentDataPath + "Screenshots/" + filename + ".png");
        }
	}
}
