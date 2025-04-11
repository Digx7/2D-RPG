using UnityEngine;
using UnityEngine.Events;

public class StringChannelRaiser : MonoBehaviour
{
    [SerializeField] private StringChannel channelToRaise;
    [SerializeField] private string m_value;

    public void Raise()
    {
        Debug.Log("StringChannelRaiser: Raise() -> " + m_value);
        channelToRaise.Raise(m_value);
    }

    public void Raise(string value)
    {
        channelToRaise.Raise(value);
    }
}
