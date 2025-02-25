using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CharacterHotBarElement : MonoBehaviour
{

    public Image actionImage;
    public TextMeshProUGUI buttonTextMeshPro;
    public TextMeshProUGUI costTextMeshPro;

    public void SetImage(Sprite sprite)
    {
        actionImage.sprite = sprite;
    }

    public void SetButton(string button)
    {
        buttonTextMeshPro.text = button;
    }

    public void SetCost(string cost)
    {
        costTextMeshPro.text = cost;
    }
}
