using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIBigNotificationWidget : UIWidget
{

    public StringChannel onNotificationChannel;
    public TextMeshProUGUI notification;
    public float lifeTime;
    
    public override void Setup(UIWidgetData newUIWidgetData)
    {
        onNotificationChannel.channelEvent.AddListener(RenderNotification);
        RenderNotification(onNotificationChannel.lastValue);
        StartCoroutine(Timer());

        base.Setup(newUIWidgetData);
    }

    public override void Teardown()
    {
        onNotificationChannel.channelEvent.RemoveListener(RenderNotification);
        
        base.Teardown();
    }

    public void RenderNotification(string message)
    {
        notification.text = message;
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(lifeTime);
        UnloadSelf();
    }
}
