using UnityEngine;
using UnityEngine.Events;

public class SceneChannelRaiser : MonoBehaviour
{
    [SerializeField] private SceneChannel channelToRaise;
    [SerializeField] private SceneData m_Data;

    public void Raise()
    {
        Debug.Log("SceneChannelRaiser: Raise()");
        channelToRaise.Raise(m_Data);
    }

    public void Raise(SceneData data)
    {
        channelToRaise.Raise(data);
    }
}
