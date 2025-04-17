using UnityEngine;
using UnityEngine.Events;

public class SceneChannelRaiser : MonoBehaviour
{
    [SerializeField] private SceneChannel channelToRaise;
    [SerializeField] private string chapter;
    [SerializeField] private string area;
    [SerializeField] private string subArea;
    [SerializeField] private SceneContext context;

    private SceneData m_Data;

    public void Awake()
    {
        m_Data.sceneName = chapter + "_" + area;
        if(subArea != "") m_Data.sceneName = m_Data.sceneName + "_" + subArea;
        m_Data.context = context;
    }

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
