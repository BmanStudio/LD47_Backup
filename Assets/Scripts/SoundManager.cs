using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider volumeSlider=null;
    public void VolumeChanged() {
        if (volumeSlider) {
            AudioListener.volume = volumeSlider.value;
        }
    }
}
