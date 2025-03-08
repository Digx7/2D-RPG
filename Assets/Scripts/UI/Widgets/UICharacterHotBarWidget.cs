using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UICharacterHotBarWidget : UIWidget
{

    public GameObject actionElementPrefab;
    public Transform actionElementParent;
    public GameObject toolTip;
    public TextMeshProUGUI toolTipAbilityName;
    public TextMeshProUGUI toolTipAbilityDescription;
    public AbilityDataListChannel abilityDataListChannel;

    private List<CombatUnit> playerCombatUnits;
    
    
    public override void Setup(UIWidgetData newUIWidgetData)
    {
        playerCombatUnits = new List<CombatUnit>();
        CombatUnit[] combatUnits = FindObjectsByType<CombatUnit>(FindObjectsSortMode.None);

        for (int i = 0; i < combatUnits.Length; i++)
        {
            if(combatUnits[i].combatFaction == CombatFaction.PLAYER) playerCombatUnits.Add(combatUnits[i]);
        }
        
        abilityDataListChannel.channelEvent.AddListener(Render);
        
        base.Setup(newUIWidgetData);

    }

    public override void Teardown()
    {
        abilityDataListChannel.channelEvent.RemoveListener(Render);

        base.Teardown();
    }

    public void Render(List<AbilityData> abilities)
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            string button = "" + (i + 1);
            SpawnActionElement(abilities[i], button);
        }
    }

    public void SpawnActionElement(AbilityData abilityData, string button)
    {
        GameObject obj = Instantiate(actionElementPrefab, actionElementParent);
        CharacterHotBarElement characterHotBarElement = obj.GetComponent<CharacterHotBarElement>();

        characterHotBarElement.SetAbility(abilityData);
        characterHotBarElement.SetButton(button);
        characterHotBarElement.SetAbilityIndex(Int32.Parse(button) - 1);
        characterHotBarElement.OnPointerClick.AddListener(OnPointerClick);

        characterHotBarElement.OnPointerEnter.AddListener(OnPointerEnterAction);
        characterHotBarElement.OnPointerExit.AddListener(OnPointerExit);
    }

    public void OnPointerEnterAction(AbilityData abilityData)
    {
        toolTip.SetActive(true);
        toolTipAbilityName.text = abilityData.AbilityName;
        toolTipAbilityDescription.text = abilityData.Description;
    }

    public void OnPointerExit(AbilityData abilityData)
    {
        toolTip.SetActive(false);
    }

    public void OnPointerClick(int abilityIndex)
    {
        foreach (CombatUnit unit in playerCombatUnits)
        {
            unit.PreviewAbility(abilityIndex);
        }
    }
}
