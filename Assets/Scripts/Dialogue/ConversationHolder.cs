using UnityEngine;
using UnityEngine.Events;

public class ConversationHolder : MonoBehaviour
{
    public Conversation conversation;
    public UIWidgetData dialogueWidgetData;
    public UIWidgetDataChannel requestLoadDialogueWidgetChannel;
    public UIWidgetDataChannel requestUnloadDialogueWidgetChannel;
    public ConversationNodeChannel onConversationUpdateChannel;

    public UnityEvent OnConversationEnd;

    private int currentNodeIndex = 0;
    private ConversationNode currentNode;
    private bool isConversationGoing = false;

    public void Interact()
    {
        Debug.Log("ConversationHolder: Interact()");

        if(!isConversationGoing)
        {
            isConversationGoing = true;
            StartConversation();
        }
        else
        {
            ProgressConversation();
        }
    }

    private void StartConversation()
    {
        Debug.Log("ConversationHolder: StartConversation()");

        currentNodeIndex = 0;
        if(TryGetNode())
        {
            requestLoadDialogueWidgetChannel.Raise(dialogueWidgetData);
            onConversationUpdateChannel.Raise(currentNode);
            currentNode.Print();
        }
        else
        {
            EndConversation();
        }
    }

    private void ProgressConversation()
    {
        Debug.Log("ConversationHolder: ProgressConversation()");
        
        currentNodeIndex++;
        if(TryGetNode())
        {
            onConversationUpdateChannel.Raise(currentNode);
            currentNode.Print();
        }
        else
        {
            EndConversation();
        }
    }

    private void EndConversation()
    {
        Debug.Log("ConversationHolder: EndConversation()");
        
        isConversationGoing = false;
        requestUnloadDialogueWidgetChannel.Raise(dialogueWidgetData);
        OnConversationEnd.Invoke();
        TryLoadNextConversation();
    }

    private bool TryGetNode()
    {
        if(currentNodeIndex < conversation.nodes.Count)
        {
            currentNode = conversation.nodes[currentNodeIndex];
            return true;
        }
        return false;
    }

    private bool TryLoadNextConversation()
    {
        if(conversation.nextConversationToLoadOnFinish != null)
        {
            conversation = conversation.nextConversationToLoadOnFinish;
            return true;
        }
        return false;
    }
}