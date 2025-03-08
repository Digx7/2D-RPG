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
    private int abilityIndex = 0;

    public BooleanChannel requestCanConfirmAbilities;

    public AbilityDataEvent OnPointerEnter;
    public AbilityDataEvent OnPointerExit;
    public IntEvent OnPointerClick;

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

    public void SetAbilityIndex(int index)
    {
        abilityIndex = index;
        // Debug.Log("" + abilityIndex);
    }

    public void RaiseOnPointerEnter()
    {
        requestCanConfirmAbilities.Raise(false);
        OnPointerEnter.Invoke(abilityData);
    }

    public void RaiseOnPointerExit()
    {
        requestCanConfirmAbilities.Raise(true);
        OnPointerExit.Invoke(abilityData);
    }

    public void RaisOnPointerClick()
    {
        // Debug.Log("Raise with " + abilityIndex);
        
        OnPointerClick.Invoke(abilityIndex);
    }
}
