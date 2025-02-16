using UnityEngine;
using UnityEngine.Events;

public class ChannelRaiser : MonoBehaviour
{
    [SerializeField] private Channel channelToRaise;

    public void Raise()
    {
        channelToRaise.Raise();
    }
}
