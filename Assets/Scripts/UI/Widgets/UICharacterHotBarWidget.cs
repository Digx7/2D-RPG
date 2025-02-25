using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UICharacterHotBarWidget : UIWidget
{

    public GameObject actionElementPrefab;
    public Transform actionElementParent;
    public AbilityDataListChannel abilityDataListChannel;
    
    public override void Setup(UIWidgetData newUIWidgetData)
    {
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
            string cost = "" + abilities[i].EnergyCost;
            SpawnActionElement(abilities[i].AbilityIcon, button, cost);
        }
    }

    public void SpawnActionElement(Sprite sprite, string button, string cost)
    {
        GameObject obj = Instantiate(actionElementPrefab, actionElementParent);
        CharacterHotBarElement characterHotBarElement = obj.GetComponent<CharacterHotBarElement>();

        characterHotBarElement.SetImage(sprite);
        characterHotBarElement.SetButton(button);
        characterHotBarElement.SetCost(cost);
    }
}
