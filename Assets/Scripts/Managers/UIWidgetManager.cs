using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIWidgetManager : Singleton<UIWidgetManager>
{
    [SerializeField] private Transform Canvas;
    [SerializeField] private List<UIWidgetData> activeWidgets;

    [SerializeField] private UIWidgetDataChannel requestLoadUIWidgetChannel;
    [SerializeField] private Channel onLoadUIWidgetChannel;
    [SerializeField] private UIWidgetDataChannel requestUnLoadUIWidgetChannel;
    [SerializeField] private Channel onUnLoadUIWidgetChannel;
    [SerializeField] private Channel requestClearAllUIWidgetsChannel;
    [SerializeField] private Channel onClearAllUIWidgetsChannel;

    // SETUP CHANNELS =======================================================

    private void OnEnable()
    {
        SetupChannels();
    }

    private void OnDisable()
    {
        TeardownChannels();
    }

    private void SetupChannels()
    {
        requestLoadUIWidgetChannel.channelEvent.AddListener(LoadWidget);
        requestUnLoadUIWidgetChannel.channelEvent.AddListener(UnloadWidget);
        requestClearAllUIWidgetsChannel.channelEvent.AddListener(UnloadAllWidgets);
    }

    private void TeardownChannels()
    {
        requestLoadUIWidgetChannel.channelEvent.RemoveListener(LoadWidget);
        requestUnLoadUIWidgetChannel.channelEvent.RemoveListener(UnloadWidget);
        requestClearAllUIWidgetsChannel.channelEvent.RemoveListener(UnloadAllWidgets);
    }

    // CHANNEL RESPONSES ===========================================================

    private void LoadWidget(UIWidgetData newWidgetData)
    {
        if(activeWidgets.Contains(newWidgetData))
        {
            Debug.LogWarning("Something tried to load the UIWidget: " + newWidgetData + " which is already loaded");
            return;
        }
        
        newWidgetData.SpawnWidget(Canvas);
        activeWidgets.Add(newWidgetData);

        onLoadUIWidgetChannel.Raise();
    }

    private void UnloadWidget(UIWidgetData widgetDataToUnload)
    {
        if(!activeWidgets.Contains(widgetDataToUnload)) return;

        widgetDataToUnload.DespawnWidget();
        activeWidgets.Remove(widgetDataToUnload);

        onUnLoadUIWidgetChannel.Raise();
    }

    private void UnloadAllWidgets()
    {
        foreach (UIWidgetData uIWidgetData in activeWidgets)
        {
            uIWidgetData.DespawnWidget();
        }

        activeWidgets.Clear();
        
        onClearAllUIWidgetsChannel.Raise();
    }
}
