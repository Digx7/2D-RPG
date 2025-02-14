using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Blackboard
{
    public Dictionary<string, GenericBlackBoardEntry> entries;

    public Blackboard()
    {
        entries = new Dictionary<string, GenericBlackBoardEntry>();
    }

    public T GetData<T>(string key)
    {
        if(!entries.ContainsKey(key))
        {
            CreateDefaultEntry<T>(key);
        }

        return entries[key].GetEntryValue<T>();
    }

    public void UpdateData<T>(string key, T value)
    {
        if(!entries.ContainsKey(key))
        {
            CreateDefaultEntry<T>(key);
        }
        
        entries[key].SetEntryValue<T>(value);
    }

    public void ClearAllData()
    {
        entries.Clear();
    }

    protected void CreateDefaultEntry<T>(string key)
    {
        entries[key] = new GenericBlackBoardEntry();
    }

    public void PrintAllEntries()
    {
        Debug.Log("Printing All Entries");
        if(entries.Count == 0) Debug.Log("No Entries found");
        
        foreach (KeyValuePair<string, GenericBlackBoardEntry> entry in entries)
        {
            string key = entry.Key;
            GenericBlackBoardEntry value = entry.Value;
            Debug.Log("" + key + " : " + value.ToString());
        }
    }
}