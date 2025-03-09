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
    public ImageColorHelper generalImageColorHelper;
    public ImageColorHelper boaderImageColorHelper;
    private AbilityData abilityData;
    private int abilityIndex = 0;
    private bool isBeingPreviewed = false;
    private bool canAfford = true;

    public BooleanChannel requestCanConfirmAbilities;

    public AbilityDataEvent OnPointerEnter;
    public AbilityDataEvent OnPointerExit;
    public IntEvent OnPointerClick;

    public void SetAbility(AbilityData newAbilityData)
    {
        abilityData = newAbilityData;

        actionImage.sprite = abilityData.AbilityIcon;
        costTextMeshPro.text = "" + abilityData.EnergyCost;

        abilityData.OnSpawnPreview.AddListener(StartPreviewing);
        abilityData.OnDespawnPreview.AddListener(StopPreviewing);
        abilityData.OnUpdateCanAfford.AddListener(SetCanAfford);
    }

    public void OnDisable()
    {
        if(abilityData != null)
        {
            abilityData.OnSpawnPreview.RemoveListener(StartPreviewing);
            abilityData.OnDespawnPreview.RemoveListener(StopPreviewing);
            abilityData.OnUpdateCanAfford.RemoveListener(SetCanAfford);
        }
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

    public void RefreshColor()
    {
        if(!canAfford)
        {
            generalImageColorHelper.SetColorIndex(1);
            boaderImageColorHelper.SetColorIndex(1);
        }
        else if(isBeingPreviewed)
        {
            generalImageColorHelper.SetColorIndex(0);
            boaderImageColorHelper.SetColorIndex(2);
        }
        else
        {
            generalImageColorHelper.SetColorIndex(0);
            boaderImageColorHelper.SetColorIndex(0);
        }
    }

    public void SetIsBeingPreviewed(bool value)
    {
        isBeingPreviewed = value;
        RefreshColor();
    }

    public void StartPreviewing()
    {
        SetIsBeingPreviewed(true);
    }

    public void StopPreviewing()
    {
        SetIsBeingPreviewed(false);
    }

    public void SetCanAfford(bool value)
    {
        canAfford = value;
        RefreshColor();
    }

    public void CheckIfCanAfford(int CurrentEnergy)
    {
        if(CurrentEnergy >= abilityData.EnergyCost)
        {
            SetCanAfford(true);
        }
        else
        {
            SetCanAfford(false);
        }
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
