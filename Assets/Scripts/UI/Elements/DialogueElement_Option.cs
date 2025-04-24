using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueElement_Option : UIElement
{
    public TextMeshProUGUI textMeshProUGUI;
    
    public IntChannel onPlayerTryInteract;
    private int m_interactValue = 1;
    
    public void SetOption(OptionPair option, int interactValue = 1)
    {
        m_interactValue = interactValue;

        textMeshProUGUI.text = interactValue + " : " + option.line;
    }

    public void OnClick()
    {
        onPlayerTryInteract.Raise(m_interactValue);
    }
}