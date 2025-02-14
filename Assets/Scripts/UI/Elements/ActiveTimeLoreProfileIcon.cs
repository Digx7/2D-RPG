using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ActiveTimeLoreProfileIcon : UIElement
{
    public ActiveTimeLore activeTimeLore;

    public Image profileImage;
    public TextMeshProUGUI titleTextMeshPro;
    public ActiveTimeLoreEvent OnSelect;

    public void Render(ActiveTimeLore newLore)
    {
        activeTimeLore = newLore;
        
        profileImage.sprite = activeTimeLore.profilePic;
        titleTextMeshPro.text = activeTimeLore.title;
    }

    public void Select()
    {
        OnSelect.Invoke(activeTimeLore);
    }
}

