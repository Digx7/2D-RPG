using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewUIWidgetDataChannel", menuName = "ScriptableObjects/Channels/UIWidgetData", order = 1)]
public class UIWidgetDataChannel : ScriptableObject
{

    public UIWidgetDataEvent channelEvent = new UIWidgetDataEvent();

    public void Raise(UIWidgetData value)
    {
        channelEvent.Invoke(value);
    }

    
}
