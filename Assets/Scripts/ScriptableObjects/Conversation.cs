using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewConversation", menuName = "ScriptableObjects/Dialogue/Conversation", order = 1)]
public class Conversation : ScriptableObject
{
    public List<ConversationNode> nodes;

    [Header("On Finish")]
    public QuestData questToGive;
    public QuestObjectiveProgress questProgressToGive; 
    public Conversation nextConversationToLoadOnFinish;
}

[System.Serializable]
public struct ConversationNode
{
    public Sprite icon;
    public string speaker;
    
    [TextAreaAttribute]
    public string line;
    public List<OptionPair> options;

    public int nextNode;

    public void Print()
    {
        Debug.Log(speaker + ":\n" + line);
    }
}

[System.Serializable]
public struct OptionPair
{
    public string line;
    public int nextNode;
}

[System.Serializable]
public class ConversationNodeEvent : UnityEvent<ConversationNode> {}