using UnityEngine;
using UnityEngine.Events;

public class ConversationHolder : MonoBehaviour
{
    public Conversation conversation;
    public UIWidgetData dialogueWidgetData;
    public UIWidgetDataChannel requestLoadDialogueWidgetChannel;
    public UIWidgetDataChannel requestUnloadDialogueWidgetChannel;
    public ConversationNodeChannel onConversationUpdateChannel;
    public Channel onStartInteractionChannel;
    public Channel onStopInteractionChannel;
    public QuestDataChannel giveQuestChannel;
    public QuestObjectiveProgressChannel progressQuestChannel;

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

    public void SetConversation(Conversation newConversation)
    {
        conversation = newConversation;
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

            onStartInteractionChannel.Raise();
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

        if(conversation.questToGive != null)
            giveQuestChannel.Raise(conversation.questToGive);

        if(conversation.questProgressToGive.objectiveName != "")
            progressQuestChannel.Raise(conversation.questProgressToGive);

        OnConversationEnd.Invoke();
        TryLoadNextConversation();

        onStopInteractionChannel.Raise();
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