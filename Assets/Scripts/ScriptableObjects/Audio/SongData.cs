using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewSong", menuName = "ScriptableObjects/Audio/Song", order = 1)]
public class SongData : ScriptableObject
{
    public string Name;
    public string Artist;
    public bool shouldLoop = true;
    public List<AudioClip> layers;
}
