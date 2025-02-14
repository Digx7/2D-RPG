using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewConversationNodeChannel", menuName = "ScriptableObjects/Channels/ConversationNode", order = 1)]
public class ConversationNodeChannel : ScriptableObject
{

    public ConversationNodeEvent channelEvent = new ConversationNodeEvent();

    public void Raise(ConversationNode value)
    {
        channelEvent.Invoke(value);
    }

    
}