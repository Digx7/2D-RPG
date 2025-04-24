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
    public Image image_boarder;
    public Image image_icon;

    public Sprite boarder_weak;
    public Sprite boarder_resistant;
    public Sprite boarder_heals;

    public Sprite icon_fire;
    public Sprite icon_air;
    public Sprite icon_earth;
    public Sprite icon_water;
    public Sprite icon_slash;
    public Sprite icon_pierce;
    public Sprite icon_bludgeon;
    public Sprite icon_life;
    public Sprite icon_light;
    public Sprite icon_dark;

    public void Setup(Modifier modifier)
    {
        if(modifier.multiplier == 2)
        {
            image_boarder.sprite = boarder_weak;
        }
        else if(modifier.multiplier == 0.5)
        {
            image_boarder.sprite = boarder_resistant;
        }
        else if(modifier.multiplier < 0)
        {
            image_boarder.sprite = boarder_heals;
        }

        switch(modifier.damageType)
        {
            case DamageType.SLASH:
                image_icon.sprite = icon_slash;
                break;
            case DamageType.PIERCE:
                image_icon.sprite = icon_pierce;
                break;
            case DamageType.BLUDGEON:
                image_icon.sprite = icon_bludgeon;
                break;
            case DamageType.FIRE:
                image_icon.sprite = icon_fire;
                break;
            case DamageType.EARTH:
                image_icon.sprite = icon_earth;
                break;
            case DamageType.AIR:
                image_icon.sprite = icon_air;
                break;
            case DamageType.WATER:
                image_icon.sprite = icon_water;
                break;
            case DamageType.LIFE:
                image_icon.sprite = icon_life;
                break;
            case DamageType.LIGHT:
                image_icon.sprite = icon_life;
                break;
            case DamageType.DARK:
                image_icon.sprite = icon_dark;
                break;
            default:
                break;
        }
    }
}

