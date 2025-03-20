using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueElement_Continue : UIElement
{
    public IntChannel onPlayerTryInteract;
    
    public void SetNode(ConversationNode node)
    {

    }

    public void OnClick()
    {
        onPlayerTryInteract.Raise(0);
    }
}