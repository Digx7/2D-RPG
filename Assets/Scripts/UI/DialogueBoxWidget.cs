using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueBoxWidget : UIWidget
{   
    public Image speakerIcon;
    public ScrollRect scrollRect;

    public GameObject dialogueLine_Parent;
    public GameObject dialogueLine_Prefab;

    public GameObject dialogueInput_Parent;
    public GameObject dialogueContinueInput_Prefab;
    public GameObject dialogueOptionInput_Prefab;

    public ConversationNodeChannel onConversationUpdateChannel;

    private ConversationNode currentNode;
    private DialogueElement_Line lastLine;
    private bool isTyping = false;
    
    public override void Setup(UIWidgetData newUIWidgetData)
    {
        onConversationUpdateChannel.channelEvent.AddListener(Render);
        base.Setup(newUIWidgetData);
    }

    public override void Teardown()
    {
        onConversationUpdateChannel.channelEvent.RemoveListener(Render);
        base.Teardown();
    }

    public void Render(ConversationNode latestNode)
    {
        currentNode = latestNode;
        speakerIcon.sprite = currentNode.icon;

        if(lastLine != null)
        {
            lastLine.OnFadeOut.Invoke();
        }

        lastLine = RenderLine();

        UpdateScrollRectContentSize();

        foreach (Transform child in dialogueInput_Parent.transform)
        {
            Destroy(child.gameObject);
        }

        if(latestNode.options.Count == 0)
            RenderContinue();
        else
            RenderOptions();
    }

    private void UpdateScrollRectContentSize()
    {
        RectTransform contentRectTransform = dialogueLine_Parent.GetComponent<RectTransform>();
        float size = contentRectTransform.rect.height;
        size += (lastLine.GetComponent<RectTransform>().rect.height + 20f);
        contentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);

        scrollRect.verticalNormalizedPosition = 0;
    }

    private DialogueElement_Line RenderLine()
    {
        GameObject obj = Instantiate(dialogueLine_Prefab, dialogueLine_Parent.transform);
        DialogueElement_Line dialogueElement_Line = obj.GetComponent<DialogueElement_Line>();
        dialogueElement_Line.SetNode(currentNode);

        return dialogueElement_Line;
    }

    private void RenderOptions()
    {
        for (int i = 0; i < currentNode.options.Count; i++)
        {
            GameObject obj = Instantiate(dialogueOptionInput_Prefab, dialogueInput_Parent.transform);
            DialogueElement_Option dialogueElement_Option = obj.GetComponent<DialogueElement_Option>();
            dialogueElement_Option.SetOption(currentNode.options[i], (i + 1)); 
        }
    }

    private void RenderContinue()
    {
        GameObject obj = Instantiate(dialogueContinueInput_Prefab, dialogueInput_Parent.transform);
        DialogueElement_Continue dialogueElement_Continue = obj.GetComponent<DialogueElement_Continue>();
        dialogueElement_Continue.SetNode(currentNode);
    }
}
