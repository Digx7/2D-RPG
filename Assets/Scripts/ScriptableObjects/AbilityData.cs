using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAbilityData", menuName = "ScriptableObjects/Abilities/Data", order = 1)]
public class AbilityData : ScriptableObject
{
    public string AbilityName;
    public int EnergyCost = 1;
    public GameObject abilityPrefab;

    private GameObject instantiatedObj;

    public GameObject GetInstantiatedAbility()
    {
        return instantiatedObj;
    }

    public void SpawnAbility(Transform parent, CombatUnit caster)
    {
        if(parent != null)
        {
            instantiatedObj = Instantiate(abilityPrefab, parent);
        }

        Ability ability = instantiatedObj.GetComponent<Ability>();
        ability.Setup(caster);
        ability.Use();
    }

    public void DespawnAbility()
    {
        Ability ability = instantiatedObj.GetComponent<Ability>();
        ability.Teardown();
    }
}