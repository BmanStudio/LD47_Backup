using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioClip[] AmbientClips;
    public AudioSource bgMusicSource;
    public AudioSource bgAmbientSource;
    int ambientIndex = 0;
    public Slider volumeSlider=null;
    [SerializeField] float bgFadeInTime=1;
    [SerializeField] float bgFadeOutTime=1;
    public static SoundManager instance;
    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
    }
    public void VolumeChanged() {
        if (volumeSlider) {
            AudioListener.volume = volumeSlider.value;
        }
    }
    void Update()
    {
        if (bgAmbientSource.time >= bgAmbientSource.clip.length) {
            ambientIndex = (ambientIndex + 1) % 2;
            bgAmbientSource.clip = AmbientClips[ambientIndex];
            bgAmbientSource.Play();
        }
    }

    public void TransitionBackgroundMusic(BackgroundMusic bg) {
        StartCoroutine(MakeTransition(clips[(int)bg]));

    }

    public IEnumerator MakeTransition(AudioClip next) {
        while (bgMusicSource.volume>0) {
            bgMusicSource.volume -= Time.deltaTime / bgFadeOutTime;
            yield return new WaitForEndOfFrame();
        }
        bgMusicSource.clip = next;
        bgMusicSource.Play();

        while (bgMusicSource.volume < 1)
        {
            bgMusicSource.volume += Time.deltaTime / bgFadeInTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public enum BackgroundMusic {
        MainLoop=0, BossMusic=1 , Victory=2
    }
}
