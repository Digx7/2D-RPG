using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TextColorHelper : UIColorHelper
{
    public List<TextMeshProUGUI> textMeshProUGUIs;

    protected override Color GetOGColor()
    {
        if(textMeshProUGUIs.Count == 0) return Color.black;
        return textMeshProUGUIs[0].color;
    }
    protected override void UpdateColor(Color newColor)
    {
        for (int i = 0; i < textMeshProUGUIs.Count; i++)
        {
            textMeshProUGUIs[i].color = newColor;
        }
    }
}
