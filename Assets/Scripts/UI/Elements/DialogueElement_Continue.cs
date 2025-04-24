using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueElement_Continue : UIElement
{
    public IntChannel onPlayerTryInteract;

    public void OnClick()
    {
        onPlayerTryInteract.Raise(1);
    }
}