using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueBoxWidget : UIWidget
{
    public float typingSpeed;
    
    public Image speakerIcon;

    public ScrollRect scrollRect;

    public GameObject dialogueElementParent;
    public GameObject dialogueElementPrefab;
    public GameObject dialogueElementPrefab_Options;
    public ConversationNodeChannel onConversationUpdateChannel;

    private ConversationNode currentNode;
    private DialogueElement lastDialogueElement;
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

        if(lastDialogueElement != null)
        {
            lastDialogueElement.OnFadeOut.Invoke();
        }

        if(currentNode.options.Count == 0)
        {
            lastDialogueElement = RenderDefault();
        }
        else
        {
            lastDialogueElement = RenderOptions();
        }

        RectTransform contentRectTransform = dialogueElementParent.GetComponent<RectTransform>();
        float size = contentRectTransform.rect.height;
        size += (lastDialogueElement.GetComponent<RectTransform>().rect.height + 20f);
        contentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);

        scrollRect.verticalNormalizedPosition = 0;

        // speakerNameTextMeshPro.text = currentNode.speaker;
        // if(isTyping) StopAllCoroutines();
        // StartCoroutine(TypeOutLine());
    }

    private DialogueElement RenderDefault()
    {
        GameObject obj = Instantiate(dialogueElementPrefab, dialogueElementParent.transform);
        DialogueElement dialogueElement = obj.GetComponent<DialogueElement>();
        dialogueElement.SetNode(currentNode);

        return dialogueElement;
    }

    private DialogueElement RenderOptions()
    {
        GameObject obj = Instantiate(dialogueElementPrefab, dialogueElementParent.transform);
        DialogueElement_Options dialogueElement_Options = obj.GetComponent<DialogueElement_Options>();
        dialogueElement_Options.SetNode(currentNode);

        return dialogueElement_Options;
    }

    // IEnumerator TypeOutLine()
    // {
    //     isTyping = true;
    //     dialogueTextMeshPro.text = "";
    //     int characterIndex = 0;

    //     while(characterIndex < currentNode.line.Length)
    //     {
    //         dialogueTextMeshPro.text += currentNode.line[characterIndex];
    //         yield return new WaitForSeconds(1f / typingSpeed);
    //         characterIndex++;
    //     }
    //     isTyping = false;
    // }
}
