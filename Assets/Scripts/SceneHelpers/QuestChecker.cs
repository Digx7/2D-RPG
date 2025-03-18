using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class QuestChecker : MonoBehaviour
{
    public QuestData questToCheck;
    public string nodeToCheck;
    public bool checkOnStart = true;

    public UnityEvent OnCheckPass;
    public UnityEvent OnCheckFail;
    public Channel onLoadedQuestsChannel;

    public void OnEnable()
    {
        onLoadedQuestsChannel.channelEvent.AddListener(Check);
    }

    public void OnDisable()
    {
        onLoadedQuestsChannel.channelEvent.RemoveListener(Check);
    }

    public void Start()
    {
        if(checkOnStart) Check();
    }

    public void Check()
    {
        QuestManager questManager = GameObject.FindFirstObjectByType<QuestManager>();

        if(questManager == null)
        {
            Fail();
            return;
        }

        if(!questManager.activeQuests.Contains(questToCheck))
        {
            Fail();
            return;
        }

        QuestNode activeNode = questToCheck.GetActiveNode();
        if(activeNode == null)
        {
            Fail();
            return;
        }

        if(activeNode.nodeName != nodeToCheck)
        {
            Fail();
            return;
        }

        Succeed();
        return;

    }

    private void Fail()
    {
        Debug.Log("QuestChecker: Check for " + questToCheck.questName + " : " + nodeToCheck + " FAILED");
        OnCheckFail.Invoke();
    }

    private void Succeed()
    {
        Debug.Log("QuestChecker: Check for " + questToCheck.questName + " : " + nodeToCheck + " SUCEEDED");
        OnCheckPass.Invoke();
    }
}