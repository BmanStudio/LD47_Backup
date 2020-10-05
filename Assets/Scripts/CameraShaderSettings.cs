using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraShaderSettings : MonoBehaviour
{

    public RawImage CameraRendererCanvas;

    public RenderTexture Rendertexture;

    public static bool UseCameraShader = false;

    public Toggle checkbox;


    public void Awake()
    {
        UpdateSettings();
        checkbox.isOn = UseCameraShader;
    }

    public void UpdateSettings()
    {
        checkbox.isOn = UseCameraShader;

        if (UseCameraShader)
        {
            Camera.main.targetTexture = Rendertexture;
            CameraRendererCanvas.gameObject.SetActive(true);
        }
        else
        {
            Camera.main.targetTexture = null;
            CameraRendererCanvas.gameObject.SetActive(false);
        }
    }

    public void ChangeSettingsButton(bool state)
    {

        UseCameraShader = state;
        UpdateSettings();
    }
}
