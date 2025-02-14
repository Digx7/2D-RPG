using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ActiveTimeLoreCard : UIElement
{
    public ActiveTimeLore activeTimeLore;

    public TextMeshProUGUI titleTextMeshPro;
    public TextMeshProUGUI detailTextMeshPro;
    public Image detailImage;

    public UnityEvent OnReturn;

    public void Render(ActiveTimeLore newLore)
    {
        activeTimeLore = newLore;
        
        titleTextMeshPro.text = activeTimeLore.title;
        detailTextMeshPro.text = activeTimeLore.info;
        detailImage.sprite = activeTimeLore.detailPic;
    }

    public void Return()
    {
        OnReturn.Invoke();
    }
}

