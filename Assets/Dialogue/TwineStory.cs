using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TwineStory : ScriptableObject
{
    public string uuid;
    public string name;
    public string creator;
    public string creatorVersion;
    public string schemaName;
    public string schemaVersion;
    public int createdAtMs;
    public List<TwinePassage> passages;
}

[System.Serializable]
public class TwinePassage
{
    public string name;
    public List<string> tags;
    public string id;
    public string text;
    public List<TwineLink> links;
    public string cleanText;
}

[System.Serializable]
public class TwineLink
{
    public string linkText;
    public string passageName;
    public string original;
}