using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ImageColorHelper : UIColorHelper
{
    public List<Image> images;

    protected override Color GetOGColor()
    {
        if(images.Count == 0) return Color.black;
        return images[0].color;
    }
    protected override void UpdateColor(Color newColor)
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].color = newColor;
        }
    }
}
