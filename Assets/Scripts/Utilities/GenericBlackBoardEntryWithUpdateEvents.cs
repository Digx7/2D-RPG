using UnityEngine;
using UnityEngine.Events;
using System;

[System.Serializable]
public class GenericBlackBoardEntryWithUpdateEvents: GenericBlackBoardEntry
{

    public UnityEvent<object> OnEntryChanged;

    public GenericBlackBoardEntryWithUpdateEvents() : base()
    {
        OnEntryChanged = new UnityEvent<object>();
    }

    public override void SetEntryValue<T>(T newValue)
    {
        base.SetEntryValue<T>(newValue);
        OnEntryChanged.Invoke(value);
    }
}