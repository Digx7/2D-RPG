using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class QuestManager : Singleton<QuestManager>
{
    public List<QuestData> activeQuests;
    public QuestDataChannel reciveQuestChannel;
    public QuestObjectiveProgressChannel tryProgressQuestChannel;

    public void OnEnable()
    {
        reciveQuestChannel.channelEvent.AddListener(GiveQuest);
        tryProgressQuestChannel.channelEvent.AddListener(TryProgressQuest);
    }

    public void OnDisable()
    {
        reciveQuestChannel.channelEvent.RemoveListener(GiveQuest);
        tryProgressQuestChannel.channelEvent.RemoveListener(TryProgressQuest);
    }

    public void GiveQuest(QuestData newQuest)
    {
        if(activeQuests.Contains(newQuest))
        {
            Debug.Log("QuestManager: Something tried to give the quest " + newQuest.questName + "\nBut that quest is already active.");
            return;
        }

        newQuest.ResetQuest();
        activeQuests.Add(newQuest);

        Debug.Log("QuestManager: Added quest: " + newQuest.ToString());
    }

    public void TryProgressQuest(QuestObjectiveProgress progress)
    {
        int indexOfFinishedQuest = -1;
        
        for (int i = 0; i < activeQuests.Count; i++)
        {
            if(activeQuests[i].TryProgress(progress))
            {
                if(activeQuests[i].IsComplete())
                {
                    indexOfFinishedQuest = i;
                }
                else
                {
                    Debug.Log("QuestManager: Progressed a quest but did not finish it");
                }
            }
        }

        if(indexOfFinishedQuest != -1)
        {
            FinishQuestAtIndex(indexOfFinishedQuest);
        }
    }

    private void FinishQuestAtIndex(int index)
    {
        QuestData finishedQuest = activeQuests[index];
        finishedQuest.Finish();

        Debug.Log("QuestManager: Finished Quest: " + finishedQuest.ToString());
        activeQuests.RemoveAt(index);
    }
}