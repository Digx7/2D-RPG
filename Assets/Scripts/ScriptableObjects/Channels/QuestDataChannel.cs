using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewQuestDataChannel", menuName = "ScriptableObjects/Channels/Quest/Data", order = 1)]
public class QuestDataChannel : ScriptableObject
{

    public QuestDataEvent channelEvent = new QuestDataEvent();

    public void Raise(QuestData value)
    {
        channelEvent.Invoke(value);
    }

    
}
