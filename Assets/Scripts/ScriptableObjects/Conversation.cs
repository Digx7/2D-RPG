using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewConversation", menuName = "ScriptableObjects/Dialogue/Conversation", order = 1)]
public class Conversation : ScriptableObject
{
    public List<ConversationNode> nodes;

    public Conversation nextConversationToLoadOnFinish;
}

[System.Serializable]
public struct ConversationNode
{
    public string speaker;
    
    [TextAreaAttribute]
    public string line;

    public void Print()
    {
        Debug.Log(speaker + ":\n" + line);
    }
}

public class ConversationNodeEvent : UnityEvent<ConversationNode> {}