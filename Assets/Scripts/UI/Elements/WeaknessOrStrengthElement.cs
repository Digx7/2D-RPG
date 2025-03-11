using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class WeaknessOrStrengthElement : UIElement
{
    public TextMeshProUGUI letterTextMeshPro;
    public Image image;
    public Color weaknessColor;
    public Color strengthColor;
    public Color healsColor;

    public void Setup(Modifier modifier)
    {
        if(modifier.multiplier > 1)
        {
            image.color = weaknessColor;
        }
        else if(modifier.multiplier < 1 && modifier.multiplier > 0)
        {
            image.color = strengthColor;
        }
        else if(modifier.multiplier < 0)
        {
            image.color = healsColor;
        }

        switch(modifier.damageType)
        {
            case DamageType.SLASH:
                letterTextMeshPro.text = "S";
                break;
            case DamageType.PIERCE:
                letterTextMeshPro.text = "P";
                break;
            case DamageType.BLUDGEON:
                letterTextMeshPro.text = "B";
                break;
            case DamageType.FIRE:
                letterTextMeshPro.text = "F";
                break;
            case DamageType.EARTH:
                letterTextMeshPro.text = "E";
                break;
            case DamageType.AIR:
                letterTextMeshPro.text = "A";
                break;
            case DamageType.WATER:
                letterTextMeshPro.text = "W";
                break;
            case DamageType.LIFE:
                letterTextMeshPro.text = "Lif";
                break;
            case DamageType.LIGHT:
                letterTextMeshPro.text = "Lig";
                break;
            case DamageType.DARK:
                letterTextMeshPro.text = "D";
                break;
            default:
                letterTextMeshPro.text = "?";
                break;
        }
    }
}

