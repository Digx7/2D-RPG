using UnityEngine;
using UnityEngine.Events;

public class UIDataChannelRaiser : MonoBehaviour
{
    [SerializeField] private UIWidgetDataChannel channelToRaise;
    [SerializeField] private UIWidgetData data;

    public void Raise()
    {
        channelToRaise.Raise(data);
    }
}
