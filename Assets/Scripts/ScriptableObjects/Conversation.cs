using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewConversation", menuName = "ScriptableObjects/Dialogue/Conversation", order = 1)]
public class Conversation : ScriptableObject
{
    [SerializeReference]
    public List<ConversationNode> nodes;

    // [SerializeReference]
    // public List<Node> nodes_2;

    // [Header("On Finish")]
    // public QuestData questToGive;
    // public QuestObjectiveProgress questProgressToGive; 
    // public Conversation nextConversationToLoadOnFinish;
}

[System.Serializable]
public class ConversationNode
{
    public string ID;
    public string LocKey;
    public string nextNode;

    public virtual void Print()
    {
        Debug.Log("BaseNode: " + ID);
    }
}

[System.Serializable]
public class ConversationNode_Line : ConversationNode
{
    public Sprite icon;
    public string speaker;
    
    [TextAreaAttribute]
    public string line;

    public override void Print()
    {
        Debug.Log("LineNode: " + speaker + " - " + line);
    }
}

[System.Serializable]
public class ConversationNode_Options : ConversationNode_Line
{
    public List<OptionPair> options;

    public override void Print()
    {
        Debug.Log("OptionsNode: " + speaker + " - " + line);
    }
}

[System.Serializable]
public class ConversationNode_End : ConversationNode
{
    public Conversation nextConversationToLoadOnFinish;
    
    public override void Print()
    {
        Debug.Log("EndNode");
    }
}

[System.Serializable]
public class ConversationNode_QuestUpdate : ConversationNode
{
    public QuestData questToGive;
    public QuestObjectiveProgress questProgressToGive;

    public override void Print()
    {
        Debug.Log("QuestNode");
    }
}

[System.Serializable]
public struct OptionPair
{
    public string line;
    public string nextNode;
}

[System.Serializable]
public class ConversationNodeEvent : UnityEvent<ConversationNode> {}