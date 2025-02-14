using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BlackboardWithUpdateEvents
{
    public Dictionary<string, GenericBlackBoardEntryWithUpdateEvents> entries;

    public BlackboardWithUpdateEvents()
    {
        entries = new Dictionary<string, GenericBlackBoardEntryWithUpdateEvents>();
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

    public void SubscribeToUpdates(string key, UnityAction<object> call)
    {
        if(!entries.ContainsKey(key))
        {
            CreateDefaultEntry<object>(key);
        }

        entries[key].OnEntryChanged.AddListener(call);
    }

    public void UnSubscribeToUpdates(string key, UnityAction<object> call)
    {
        if(!entries.ContainsKey(key))
        {
            CreateDefaultEntry<object>(key);
        }

        entries[key].OnEntryChanged.RemoveListener(call);
    }

    protected void CreateDefaultEntry<T>(string key)
    {
        entries[key] = new GenericBlackBoardEntryWithUpdateEvents();
    }

    public void PrintAllEntries()
    {
        Debug.Log("Printing All Entries");
        if(entries.Count == 0) Debug.Log("No Entries found");
        
        foreach (KeyValuePair<string, GenericBlackBoardEntryWithUpdateEvents> entry in entries)
        {
            string key = entry.Key;
            GenericBlackBoardEntryWithUpdateEvents value = entry.Value;
            Debug.Log("" + key + " : " + value.ToString());
        }
    }
}