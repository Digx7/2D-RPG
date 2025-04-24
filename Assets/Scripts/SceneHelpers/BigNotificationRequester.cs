using UnityEngine;

public class BigNotificationRequester : MonoBehaviour
{
    public string notification;
    public UIWidgetDataChannel requestLoadUIWidgetChannel;
    public StringChannel requestBigNotificationChannel;
    public UIWidgetData notificationWidgetData;

    public void Notify()
    {
        Debug.Log("BigNotificationRequester: Notify()");
        requestBigNotificationChannel.Raise(notification);
        requestLoadUIWidgetChannel.Raise(notificationWidgetData);
    }
}
