using UnityEngine;
using UnityEngine.Events;
using System;

[System.Serializable]
public class GenericBlackBoardEntry 
{
    public object value;

    public GenericBlackBoardEntry()
    {
        value = new object();
    }

    public virtual T GetEntryValue<T>()
    {
        return (T)value;
    }

    public virtual void SetEntryValue<T>(T newValue)
    {
        value = newValue;
    }

    public virtual string ToString()
    {
        return ("" + value);
    }
}