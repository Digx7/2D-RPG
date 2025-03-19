using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueOption : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public Button button;

    public void SetPair(OptionPair pair)
    {
        textMeshProUGUI.text = pair.line;
    }
}