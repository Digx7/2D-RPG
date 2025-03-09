using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAbilityData", menuName = "ScriptableObjects/Abilities/Data", order = 1)]
public class AbilityData : ScriptableObject
{
    public string AbilityName;
    public int EnergyCost = 1;
    public Sprite AbilityIcon;

    [TextArea]
    public string Description;
    public GameObject abilityPrefab;
    public GameObject previewPrefab;
    public UnityEvent OnSpawnAbility;
    public UnityEvent OnSpawnPreview;
    public UnityEvent OnDespawnPreview;
    public BooleanEvent OnUpdateCanAfford;

    private GameObject instantiatedObj;

    public GameObject GetInstantiatedAbility()
    {
        return instantiatedObj;
    }

    public void SpawnAbility(Transform parent, CombatUnit caster, AbilityUsageContext abilityUsageContext)
    {
        if(parent != null)
        {
            instantiatedObj = Instantiate(abilityPrefab, parent);
        }

        Ability ability = instantiatedObj.GetComponent<Ability>();

        OnSpawnAbility.Invoke();
        ability.Setup(caster, abilityUsageContext);
        ability.Use();
    }

    public void DespawnAbility()
    {
        Ability ability = instantiatedObj.GetComponent<Ability>();
        ability.Teardown();
    }

    public void SpawnPreview(Transform parent, CombatUnit caster)
    {
        if(parent != null)
        {
            instantiatedObj = Instantiate(previewPrefab, parent);
        }

        AbilityPreview abilityPreview;
        if(instantiatedObj.TryGetComponent<AbilityPreview>(out abilityPreview))
        {
            OnSpawnPreview.Invoke();
            
            abilityPreview.Setup(caster);
            abilityPreview.Use();
        }
        
    }

    public void DespawnPreview()
    {
        AbilityPreview abilityPreview;
        if(instantiatedObj.TryGetComponent<AbilityPreview>(out abilityPreview))
            {
                OnDespawnPreview.Invoke();
                abilityPreview.Teardown();
            }
    }

    public bool Validate(AbilityUsageContext abilityUsageContext)
    {
        AbilityPreview abilityPreview;
        if(instantiatedObj.TryGetComponent<AbilityPreview>(out abilityPreview))
            return abilityPreview.Validate(abilityUsageContext);

        return false;
    }

    public void RenderSelectionUI(AbilityUsageContext abilityUsageContext)
    {
        AbilityPreview abilityPreview;
        if(instantiatedObj.TryGetComponent<AbilityPreview>(out abilityPreview))
            abilityPreview.RenderSelectionUI(abilityUsageContext);
    }
}

[System.Serializable]
public class AbilityDataEvent : UnityEvent<AbilityData> {}

[System.Serializable]
public class AbilityDataListEvent : UnityEvent<List<AbilityData>> {}

