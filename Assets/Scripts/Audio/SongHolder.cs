using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SongHolder : MonoBehaviour
{
    [SerializeField] private SongData songData;
    public SongData GetSongData(){return songData;}
    [SerializeField] private GameObject layerPrefab;


    private const float MAXFADEOUTDURATION = 3f;

    private MusicManager musicManager;
    private List<SongLayerHolder> layers;

    private float runtime;
    private float songLength;
    private bool isPlaying = false;

    public void Setup(SongData newSongData, MusicManager newMusicManager)
    {
        songData = newSongData;
        musicManager = newMusicManager;

        layers = new List<SongLayerHolder>();

        for (int i = 0; i < songData.layers.Count; i++)
        {
            GameObject obj = Instantiate(layerPrefab, this.transform);
            SongLayerHolder layer = obj.GetComponent<SongLayerHolder>();

            layer.Setup(i, songData.layers[i], songData.shouldLoop);
            layers.Add(layer);
        }

        songLength = songData.layers[0].length;
    }

    public void Update()
    {
        CheckIfSongIsEnding();
    }

    public void StartSong()
    {
        isPlaying = true;
        foreach (SongLayerHolder layer in layers)
        {
            layer.StartLayer();
        }
        layers[0].FadeIn();
    }

    public void StopSong()
    {
        isPlaying = false;
        foreach (SongLayerHolder layer in layers)
        {
            layer.StopLayer();
        }
        FadeOut();
    }

    public void PauseOrResume()
    {
        isPlaying = !isPlaying;
        foreach (SongLayerHolder layer in layers)
        {
            layer.PauseOrResume();
        }
    }

    public void AddLayer(int layerNumber)
    {
        if(layerNumber >= layers.Count)
        {
            Debug.LogWarning("A song just tried to add a layer that does not exist");
            return;
        }

        layers[layerNumber].FadeIn();
    }

    public void RemoveLayer(int layerNumber)
    {
        if(layerNumber >= layers.Count)
        {
            Debug.LogWarning("A song just tried to remove a layer that does not exist");
            return;
        }

        layers[layerNumber].FadeOut();
    }

    private void FadeOut()
    {
        StartCoroutine(FadeOutEnum());
    }

    IEnumerator FadeOutEnum()
    {
        yield return new WaitForSeconds(MAXFADEOUTDURATION);
        Destroy(this.gameObject);
    }

    private void CheckIfSongIsEnding()
    {
        // If we are looping there is no point in checking if the song is ending
        if(isPlaying && !songData.shouldLoop)
        {
            runtime += Time.deltaTime;
            if(runtime >= songLength)
            {
                musicManager.PlayNextSongInQueue();
            }
        }
    }
}
