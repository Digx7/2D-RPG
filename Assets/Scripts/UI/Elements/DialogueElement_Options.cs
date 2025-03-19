using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueElement_Options : DialogueElement
{
    public GameObject dialogueOptionParent;
    public GameObject dialogueOptionPrefab;

    public override void SetNode(ConversationNode node)
    {
        foreach (OptionPair pair in node.options)
        {
            RenderOption(pair);
        }
        
        base.SetNode(node);
    }

    protected override void Resize(int lineLength)
    {
        base.Resize(lineLength);
    }

    private void RenderOption(OptionPair pair)
    {
        GameObject obj = Instantiate(dialogueOptionPrefab, dialogueOptionParent.transform);
        DialogueOption dialogueOption = obj.GetComponent<DialogueOption>();
        dialogueOption.SetPair(pair);
    }
}