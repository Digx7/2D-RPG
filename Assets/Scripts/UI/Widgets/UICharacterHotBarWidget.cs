using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UICharacterHotBarWidget : UIWidget
{

    [Header("Actions")]
    public GameObject actionElementPrefab;
    public Transform actionElementParent;
    public GameObject toolTip;
    public TextMeshProUGUI toolTipAbilityName;
    public TextMeshProUGUI toolTipAbilityDescription;
    public AbilityDataListChannel abilityDataListChannel;

    [Header("Inspector")]
    public HealthBarElement healthBarElement;
    public UITurnOrderIcon uITurnOrderIcon;
    public WeaknessOrStrengthHolderElement weaknessOrStrengthHolderElement;
    public EnergyElement energyElement;
    public CombatUnitChannel onFocusedCombatUnitChannel;

    private List<CombatUnit> playerCombatUnits;
    private List<CharacterHotBarElement> characterHotBarElements;
    private CombatUnit focusedUnit;
    
    
    public override void Setup(UIWidgetData newUIWidgetData)
    {
        playerCombatUnits = new List<CombatUnit>();
        CombatUnit[] combatUnits = FindObjectsByType<CombatUnit>(FindObjectsSortMode.None);

        for (int i = 0; i < combatUnits.Length; i++)
        {
            if(combatUnits[i].combatFaction == CombatFaction.PLAYER) playerCombatUnits.Add(combatUnits[i]);
        }
        


        abilityDataListChannel.channelEvent.AddListener(RenderAbilities);
        onFocusedCombatUnitChannel.channelEvent.AddListener(RenderInspector);
        
        base.Setup(newUIWidgetData);

    }

    public override void Teardown()
    {
        abilityDataListChannel.channelEvent.RemoveListener(RenderAbilities);
        onFocusedCombatUnitChannel.channelEvent.RemoveListener(RenderInspector);

        if(focusedUnit != null) focusedUnit.OnEnergyUpdate_Absolute.RemoveListener(energyElement.SetEnergy);

        base.Teardown();
    }

    public void RenderAbilities(List<AbilityData> abilities)
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            string button = "" + (i + 1);
            SpawnActionElement(abilities[i], button);
        }
    }

    public void SpawnActionElement(AbilityData abilityData, string button)
    {
        characterHotBarElements = new List<CharacterHotBarElement>();
        
        GameObject obj = Instantiate(actionElementPrefab, actionElementParent);
        CharacterHotBarElement characterHotBarElement = obj.GetComponent<CharacterHotBarElement>();

        characterHotBarElement.SetAbility(abilityData);
        characterHotBarElement.SetButton(button);
        characterHotBarElement.SetAbilityIndex(Int32.Parse(button) - 1);
        characterHotBarElement.OnPointerClick.AddListener(OnPointerClick);

        characterHotBarElement.OnPointerEnter.AddListener(OnPointerEnterAction);
        characterHotBarElement.OnPointerExit.AddListener(OnPointerExit);

        if(focusedUnit != null)
        {
            characterHotBarElement.CheckIfCanAfford(focusedUnit.CurrentEnergy);
        }

        characterHotBarElements.Add(characterHotBarElement);
    }

    public void RenderInspector(CombatUnit combatUnit)
    {
        focusedUnit = combatUnit;
        
        Health health = focusedUnit.gameObject.GetComponent<Health>();
        healthBarElement.health = health;
        healthBarElement.Setup();

        weaknessOrStrengthHolderElement.health = health;
        weaknessOrStrengthHolderElement.Render();

        // uITurnOrderIcon.Render(focusedUnit.TurnOrderIcon);
        uITurnOrderIcon.SetCombatUnit(focusedUnit);

        energyElement.SetEnergy(focusedUnit.CurrentEnergy);
        focusedUnit.OnEnergyUpdate_Absolute.AddListener(energyElement.SetEnergy);

        if(characterHotBarElements != null)
        {
            foreach (CharacterHotBarElement element in characterHotBarElements)
            {
                element.CheckIfCanAfford(focusedUnit.CurrentEnergy);
            }
        }
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
