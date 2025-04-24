using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class QuestManager : Singleton<QuestManager>
{
    public List<QuestData> activeQuests;
    public QuestDataChannel reciveQuestChannel;
    public QuestObjectiveProgressChannel tryProgressQuestChannel;
    public Channel onGameLoadedChannel;
    public Channel onLoadedQuestsChannel;

    private SaveSystem saveSystem;

    public void OnEnable()
    {
        reciveQuestChannel.channelEvent.AddListener(GiveQuest);
        tryProgressQuestChannel.channelEvent.AddListener(TryProgressQuest);
        onGameLoadedChannel.channelEvent.AddListener(LoadInProgressQuests);
    }

    public void OnDisable()
    {
        reciveQuestChannel.channelEvent.RemoveListener(GiveQuest);
        tryProgressQuestChannel.channelEvent.RemoveListener(TryProgressQuest);
        onGameLoadedChannel.channelEvent.RemoveListener(LoadInProgressQuests);
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
        SaveInProgressQuest(newQuest);

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
                    SaveInProgressQuest(activeQuests[i]);
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
        SaveFinishedQuest(finishedQuest);

        Debug.Log("QuestManager: Finished Quest: " + finishedQuest.ToString());
        activeQuests.RemoveAt(index);
    }

    private void SaveInProgressQuest(QuestData quest)
    {
        if(saveSystem == null)
        {
            if(!FindSaveSystem()) return;
        }

        string questName = quest.questName;
        string activeNodeName = quest.GetActiveNode().nodeName;

        saveSystem.save.SaveQuest(questName, activeNodeName);
    }

    private void SaveFinishedQuest(QuestData quest)
    {
        if(saveSystem == null)
        {
            if(!FindSaveSystem()) return;
        }

        string questName = quest.questName;

        saveSystem.save.FinishQuest(questName);
    }

    public void LoadInProgressQuests()
    {
        if(saveSystem == null)
        {
            if(!FindSaveSystem()) return;
        }

        Dictionary<string,string> loadedQuests = saveSystem.save.GetSavedQuests();

        foreach (KeyValuePair<string,string> pair in loadedQuests)
        {
            Debug.Log("Loaded Quest: " + pair.Key + " : " + pair.Value);

            QuestData quest = Resources.Load<QuestData>(pair.Key);
            if(quest == null) continue;
            quest.SetActiveNode(pair.Value);

            activeQuests.Add(quest);
        }

        onLoadedQuestsChannel.Raise();
    }

    private bool FindSaveSystem()
    {
        saveSystem = GameObject.FindFirstObjectByType<SaveSystem>();
        if(saveSystem == null) return false;
        return true;
    }
}