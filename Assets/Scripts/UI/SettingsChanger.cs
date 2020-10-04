using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsChanger : MonoBehaviour
{
    public Slider mouseSen;
    void Start()
    {
        if (mouseSen != null) {
            mouseSen.value = Settings.MouseSensitivity;
        }
    }
    public void UpdateMouseSensitivity() {
        Settings.MouseSensitivity = mouseSen.value;
    }
}
