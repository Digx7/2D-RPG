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

    public UnityEvent OnConversationStart;
    public UnityEvent OnConversationEnd;

    private string currentNodeID = "0";
    private ConversationNode currentNode;
    private Conversation nextConversationToLoad;
    private bool isConversationGoing = false;

    public void Interact(int value)
    {
        Debug.Log("ConversationHolder: Interact()");

        if(!isConversationGoing && value == 1)
        {
            StartConversation();
        }
        else
        {
            ProgressConversation(value);
        }
    }

    public void SetConversation(Conversation newConversation)
    {
        conversation = newConversation;
    }

    private void StartConversation()
    {
        Debug.Log("ConversationHolder: StartConversation()");

        currentNodeID = "0";
        if(TryGetNode())
        {
            isConversationGoing = true;
            requestLoadDialogueWidgetChannel.Raise(dialogueWidgetData);
            onStartInteractionChannel.Raise();
            OnConversationStart.Invoke();

            OnGetNewNode();
        }
        else
        {
            EndConversation();
        }
    }

    private void ProgressConversation(int value)
    {
        Debug.Log("ConversationHolder: ProgressConversation()");
        
        // if(value == 0)
        //     currentNodeID = conversation.nodes[currentNodeID].nextNode;
        // else if(value <= conversation.nodes[currentNodeID].options.Count)
        // {
        //     currentNodeID = conversation.nodes[currentNodeID].options[(value - 1)].nextNode;
        // }
        // else
        // {
        //     return;
        // }

        // if(!TryGetNextID(value)) return;
        
        if(TryGetNextID(value) && TryGetNode())
        {
            OnGetNewNode();
        }
        else
        {
            EndConversation();
        }
    }

    private void OnGetNewNode()
    {
        currentNode.Print();
            
        if(currentNode is ConversationNode_End)
        {
            nextConversationToLoad = ((ConversationNode_End)currentNode).nextConversationToLoadOnFinish;
            EndConversation();
            return;
        }
        else if(currentNode is ConversationNode_QuestUpdate)
        {
            ConversationNode_QuestUpdate questNode = (ConversationNode_QuestUpdate) currentNode;
            
            if(questNode.questToGive != null)
                giveQuestChannel.Raise(questNode.questToGive);

            if(questNode.questProgressToGive.objectiveName != "")
                progressQuestChannel.Raise(questNode.questProgressToGive);

            ProgressConversation(0);
            return;
        }
        
        onConversationUpdateChannel.Raise(currentNode);
    }

    private void EndConversation()
    {
        Debug.Log("ConversationHolder: EndConversation()");
        
        isConversationGoing = false;
        requestUnloadDialogueWidgetChannel.Raise(dialogueWidgetData);
        onStopInteractionChannel.Raise();
        OnConversationEnd.Invoke();

        TryLoadNextConversation();

        // if(conversation.questToGive != null)
        //     giveQuestChannel.Raise(conversation.questToGive);

        // if(conversation.questProgressToGive.objectiveName != "")
        //     progressQuestChannel.Raise(conversation.questProgressToGive);

        

        
    }

    private bool TryGetNode()
    {
        // if(currentNodeID < conversation.nodes.Count)
        // {
        //     currentNode = conversation.nodes[currentNodeID];
        //     return true;
        // }
        // return false;

        for (int i = 0; i < conversation.nodes.Count; i++)
        {
            if(conversation.nodes[i].ID == currentNodeID) 
            {
                currentNode = conversation.nodes[i];
                return true;
            }
        }

        return false;
    }

    private bool TryGetNextID(int value)
    {
        if(currentNode is ConversationNode_Options)
        {
            ConversationNode_Options optionsNode = (ConversationNode_Options) currentNode;

            if(value <= optionsNode.options.Count)
            {
                currentNodeID = optionsNode.options[value - 1].nextNode;
                return true;
            }
            else return false;
        }
        else
        {
            currentNodeID = currentNode.nextNode;
            return true;
        }
    }

    private bool TryLoadNextConversation()
    {
        if(nextConversationToLoad != null)
        {
            conversation = nextConversationToLoad;
            return true;
        }
        return false;
    }
}