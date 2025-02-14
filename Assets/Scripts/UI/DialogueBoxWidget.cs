using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueBoxWidget : UIWidget
{
    public float typingSpeed;
    public TextMeshProUGUI speakerNameTextMeshPro;
    public TextMeshProUGUI dialogueTextMeshPro;
    public ConversationNodeChannel onConversationUpdateChannel;

    private ConversationNode currentNode;
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

        speakerNameTextMeshPro.text = currentNode.speaker;
        if(isTyping) StopAllCoroutines();
        StartCoroutine(TypeOutLine());
    }

    IEnumerator TypeOutLine()
    {
        isTyping = true;
        dialogueTextMeshPro.text = "";
        int characterIndex = 0;

        while(characterIndex < currentNode.line.Length)
        {
            dialogueTextMeshPro.text += currentNode.line[characterIndex];
            yield return new WaitForSeconds(1f / typingSpeed);
            characterIndex++;
        }
        isTyping = false;
    }
}
