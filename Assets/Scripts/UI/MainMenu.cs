using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject settings, menu,pauseScreen;
    public void OpenSettings() {
        menu.SetActive(false);
        settings.SetActive( true);

    }

    public void OpenMain()
    {
        menu.SetActive(true);
        settings.SetActive(false);
    }

    public void ClosePause() {
        pauseScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            if (pauseScreen.activeSelf)
                Cursor.lockState = CursorLockMode.Locked;
            else 
            Cursor.lockState = CursorLockMode.None;

            pauseScreen.SetActive(!pauseScreen.activeSelf);

        }
    }
}
