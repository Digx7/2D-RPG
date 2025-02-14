using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] List<SongData> songQueue;
    [SerializeField] GameObject songPrefab;
    [SerializeField] SongHolder activeSong;


    [SerializeField] SongChannel requestJumpToSongChannel;
    [SerializeField] Channel onJumpToSongChannel;
    [SerializeField] SongChannel requestQueueSongChannel;
    [SerializeField] Channel onQueueSongChannel;
    [SerializeField] Channel requestSkipSongChannel;
    [SerializeField] Channel onSkipSongChannel;
    [SerializeField] Channel requestPauseOrResumeSongChannel;
    [SerializeField] Channel onPauseOrResumeSongChannel;
    [SerializeField] IntChannel requestAddSongLayerChannel;
    [SerializeField] Channel onAddSongLayerChannel;
    [SerializeField] IntChannel requestRemoveSongLayerChannel;
    [SerializeField] Channel onRemoveSongLayerChannel;

    // CHANELS =================================

    private void OnEnable()
    {
        SetupChannels();
    }

    private void OnDisable()
    {
        TearDownChannels();
    }

    private void SetupChannels()
    {
        requestJumpToSongChannel.channelEvent.AddListener(JumpToSong);
        requestQueueSongChannel.channelEvent.AddListener(QueueSong);
        requestSkipSongChannel.channelEvent.AddListener(SkipSong);
        requestPauseOrResumeSongChannel.channelEvent.AddListener(PauseOrResume);
        requestAddSongLayerChannel.channelEvent.AddListener(AddSongLayer);
        requestRemoveSongLayerChannel.channelEvent.AddListener(RemoveSongLayer);
    }

    private void TearDownChannels()
    {
        requestJumpToSongChannel.channelEvent.RemoveListener(JumpToSong);
        requestQueueSongChannel.channelEvent.RemoveListener(QueueSong);
        requestSkipSongChannel.channelEvent.RemoveListener(SkipSong);
        requestPauseOrResumeSongChannel.channelEvent.RemoveListener(PauseOrResume);
        requestAddSongLayerChannel.channelEvent.RemoveListener(AddSongLayer);
        requestRemoveSongLayerChannel.channelEvent.RemoveListener(RemoveSongLayer);
    }

    // CHANNEL RESPONSES =================================

    private void JumpToSong(SongData song)
    {
        PlayNewSong(song);
        onJumpToSongChannel.Raise();
    }

    private void QueueSong(SongData song)
    {
        songQueue.Add(song);
        onQueueSongChannel.Raise();

        if(!IsThereAnActiveSong()) PlayNextSongInQueue();
    } 

    private void SkipSong()
    {
        PlayNextSongInQueue();
        onSkipSongChannel.Raise();
    }

    private void PauseOrResume()
    {
        if(activeSong != null) activeSong.PauseOrResume();
        onPauseOrResumeSongChannel.Raise();
    }

    private void AddSongLayer(int layerNumber)
    {
        if(activeSong != null) 
        {
            activeSong.AddLayer(layerNumber);
            onAddSongLayerChannel.Raise();
        }
    }

    private void RemoveSongLayer(int layerNumber)
    {
        if(activeSong != null) 
        {
            activeSong.RemoveLayer(layerNumber);
            onRemoveSongLayerChannel.Raise();
        }
    }

    // MAIN FUNCTIONS =================================

    public void PlayNextSongInQueue()
    {
        if(songQueue.Count == 0)
        {
            Debug.Log("The Music Manager tried playing the next song in its queue, but the queue is empty");
            activeSong.StopSong();
            activeSong = null;
            return;
        }

        PlayNewSong(songQueue[0]);
        songQueue.RemoveAt(0);
    }

    private void PlayNewSong(SongData song)
    {
        if(IsThereAnActiveSong())
        {
            // check is this song is already playing
            if(activeSong.GetSongData() == song)
            {
                Debug.LogWarning("We just tried to play the same song that already is playing");
                return;
            }
            // If a song is active stop it
            activeSong.StopSong();
        }
        
        // load this song
        GameObject loaded = Instantiate(songPrefab, this.transform);
        activeSong = loaded.GetComponent<SongHolder>();
        activeSong.Setup(song, this);
        // Start this song
        activeSong.StartSong();
    }

    private bool IsThereAnActiveSong()
    {
        if(activeSong != null) return true;
        else return false;
    }
}
