using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharacterHotBarRequester : MonoBehaviour
{
    public CombatUnit combatUnit;
    public UIWidgetData CharacterHotbarWidget;
    public UIWidgetDataChannel requestLoadUIChannel;
    public UIWidgetDataChannel requestUnLoadUIChannel;
    public AbilityDataListChannel abilitiesListChannel;
    public CombatUnitChannel onFocusedCombatUnitChannel;


    public void OnStartTurn()
    {
        StartCoroutine(StartTurn());
    }

    public void OnEndTurn()
    {
        requestUnLoadUIChannel.Raise(CharacterHotbarWidget);
    }

    IEnumerator StartTurn()
    {
        requestLoadUIChannel.Raise(CharacterHotbarWidget);
        yield return null;
        List<AbilityData> abilities = combatUnit.abilities;
        abilitiesListChannel.Raise(abilities);
        onFocusedCombatUnitChannel.Raise(combatUnit);
    }
}


