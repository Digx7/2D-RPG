using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UICombatLogWidget : UIWidget
{
    public GameObject LogEntryPrefab;
    public GameObject LogEntryParent;
    public float LogEntryHeight = 25f;
    public RectTransform contentRectTransform;
    public StringChannel OnCombatLogChannel;

    public override void Setup(UIWidgetData newUIWidgetData)
    {
        base.Setup(newUIWidgetData);

        OnCombatLogChannel.channelEvent.AddListener(AddLog);
    }

    public override void Teardown()
    {
        OnCombatLogChannel.channelEvent.RemoveListener(AddLog);
        
        base.Teardown();
    }

    public void AddLog(string newLogMessage)
    {
        GameObject obj = Instantiate(LogEntryPrefab, LogEntryParent.transform);
        obj.transform.SetSiblingIndex(0);

        LogEntryElement logEntryElement = obj.GetComponent<LogEntryElement>();
        logEntryElement.SetEntry(newLogMessage);

        // Increase scroll view height based 
        float size = contentRectTransform.rect.height;
        size += LogEntryHeight;
        contentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
    }
}
