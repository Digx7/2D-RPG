using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewQuestObjectiveProgressChannel", menuName = "ScriptableObjects/Channels/Quest/ObjectiveProgress", order = 1)]
public class QuestObjectiveProgressChannel : ScriptableObject
{

    public QuestObjectiveProgressEvent channelEvent = new QuestObjectiveProgressEvent();

    public void Raise(QuestObjectiveProgress value)
    {
        channelEvent.Invoke(value);
    }

    
}
