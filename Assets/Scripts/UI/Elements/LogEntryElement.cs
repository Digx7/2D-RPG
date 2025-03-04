using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class LogEntryElement : UIElement
{
    public TextMeshProUGUI textMeshProUGUI;

    public void SetEntry(string entry)
    {
        textMeshProUGUI.text = entry;
    }
}

