using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLockToggle : MonoBehaviour
{
    public KeyCode cursorToggle = KeyCode.T;
    public bool cursorIsLocked;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cursorIsLocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(cursorToggle))
        {
            if (cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                cursorIsLocked = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                cursorIsLocked = true;
            }
        }
    }
}
