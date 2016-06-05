using UnityEngine;
using System.Collections;
using System;

public class MouseManager : MonoBehaviour {

    public KeyCode togglePointerCode = KeyCode.LeftControl;
    private bool toggle = false;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

	void Update () {
	
        if (Input.GetKeyUp(togglePointerCode))
        {
            toggleCursor();
        }

        if (toggle)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    private void toggleCursor()
    {
        toggle = !toggle;
    }
}
