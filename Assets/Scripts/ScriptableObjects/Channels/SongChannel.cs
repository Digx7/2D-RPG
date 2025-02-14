using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewSongChannel", menuName = "ScriptableObjects/Channels/Song", order = 1)]
public class SongChannel : ScriptableObject
{

    public SongEvent channelEvent = new SongEvent();

    public void Raise(SongData value)
    {
        channelEvent.Invoke(value);
    }

    
}