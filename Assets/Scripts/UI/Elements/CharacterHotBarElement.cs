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
    private AbilityData abilityData;

    public AbilityDataEvent OnPointerEnter;
    public AbilityDataEvent OnPointerExit;

    public void SetAbility(AbilityData newAbilityData)
    {
        abilityData = newAbilityData;

        actionImage.sprite = abilityData.AbilityIcon;
        costTextMeshPro.text = "" + abilityData.EnergyCost;
    }

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

    public void RaiseOnPointerEnter()
    {
        OnPointerEnter.Invoke(abilityData);
    }

    public void RaiseOnPointerExit()
    {
        OnPointerExit.Invoke(abilityData);
    }
}
