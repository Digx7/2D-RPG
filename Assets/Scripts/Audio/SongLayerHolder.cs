using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SongLayerHolder : MonoBehaviour
{
    [SerializeField] private int layerNumber;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource audioSource;

    private const float FADEINSPEED = 0.5f;
    private const float FADEOUTSPEED = 0.5f;
    

    public void Setup(int newLayerNumber, AudioClip newClip, bool shouldLoop = false)
    {
        layerNumber = newLayerNumber;
        clip = newClip;
        audioSource.clip = clip;
        audioSource.volume = 0;
        audioSource.loop = shouldLoop;
    }

    public void StartLayer()
    {
        audioSource.Play();
    }

    public void StopLayer()
    {
        FadeOut();
    }

    public void PauseOrResume()
    {
        if(audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInEnum());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutEnum());
    }

    IEnumerator FadeInEnum()
    {
        while(audioSource.volume < 1.0f)
        {
            SetVolume(audioSource.volume + (FADEINSPEED * Time.deltaTime));
            yield return null;
        }
        SetVolume(1.0f);
    }

    IEnumerator FadeOutEnum()
    {
        while(audioSource.volume > 0.0f)
        {
            SetVolume(audioSource.volume - (FADEINSPEED * Time.deltaTime));
            yield return null;
        }
        SetVolume(0.0f);
    }
}
