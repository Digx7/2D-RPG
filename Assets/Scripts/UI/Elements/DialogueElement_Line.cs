using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueElement_Line : UIElement
{
    public TextMeshProUGUI lineTextMeshPro;
    public UnityEvent OnFadeOut;

    public void SetLine(string line)
    {
        lineTextMeshPro.text = line;

        Resize(line.Length);
    }

    protected void Resize(int lineLength)
    {
        RectTransform contentRectTransform = gameObject.GetComponent<RectTransform>();
        float size = contentRectTransform.rect.height;
        size += ((lineLength/40) * 50f);
        contentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
    }
}