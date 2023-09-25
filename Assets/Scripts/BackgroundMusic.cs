using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;
    public static BackgroundMusic current;
    public AudioClip defaultMusic;
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }
    public void playDefaultMusic()
    {
        changeMusic(2f, defaultMusic);
    }
    public void changeMusic(float duration,AudioClip newClip)
    {
        StartCoroutine(AudioFade.FadeOutIn(audioSource, duration, newClip));
    }
    public void setVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
