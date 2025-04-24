using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;

public class CombatUnit : MonoBehaviour
{
    public string UnitName;
    public int CurrentEnergy;
    public int DefaultEnergy = 2;
    public UnitStats Stats;
    public List<AbilityData> abilities;
    public CombatFaction combatFaction;
    public Sprite TurnOrderIcon;
    public Animator animator;
    public string HurtAnimationName;
    public string HealAnimationName;
    public string DeathAnimationName;
    public string ReviveAnimationName;
    public Channel onEndUnitTurnChannel;
    public UnityEvent onStartTurn;
    public UnityEvent onStillHasEnergyLeft;
    public UnityEvent onEndTurn;
    public StringEvent onUseAbility;
    public UnityEvent onDeath;
    public UnityEvent onDestroy;
    public IntEvent OnEnergyUpdate_Absolute;
    public IntEvent OnEnergyUpdate_Delta;
    public StringChannel OnCombatLogChannel;
    public Vector3Channel RequestFocusUnitChannel;
    public CombatUnitChannel RequestStartFollowingUnitChannel;
    public Channel RequestStopFollowingUnitChannel;
    public bool followUnit = false;

    private bool isTurn = false;
    private bool isUsingAbility = false;
    private bool isPreviewingAbility = false;
    private int previewIndex = -1;
    public bool IsDead {get; private set;} = false;

    private void OnEnable()
    {
        CurrentEnergy = 0;
        OnEnergyUpdate_Absolute.Invoke(CurrentEnergy);
    }

    private void OnDisable()
    {
    }

    public void Prompt()
    {
        // Done to avoid an infinite call stack

        StartCoroutine(onPrompt());
    }

    public void EndTurnIfEnergyIsOut()
    {
        if(!isTurn) return;

        isUsingAbility = false;

        if(CurrentEnergy <= 0) EndTurn();
        else onStillHasEnergyLeft.Invoke();
    }

    public void EndTurn()
    {
        if(!isTurn) return;

        Debug.Log("CombatUnit: " + UnitName + " ending turn");

        if(followUnit) RequestStopFollowingUnitChannel.Raise();

        isTurn = false;
        isUsingAbility = false;
        onEndUnitTurnChannel.Raise();
        onEndTurn.Invoke();
    }

    public void PreviewAbility(int index)
    {
        if(IsDead) return;

        if(!isTurn) return;

        if(isUsingAbility) return;
        
        if(index >= abilities.Count) return; 
        
        if(isPreviewingAbility) StopPreviewing();

        if(abilities[index].EnergyCost <= CurrentEnergy)
        {
            Debug.Log("CombatUnit: " + UnitName + " is previewing ability " + (index + 1) + " " + abilities[index].AbilityName);

            abilities[index].SpawnPreview(transform, this);

            isPreviewingAbility = true;
            previewIndex = index;
        }       
    }

    public void RenderPreviewSelection(AbilityUsageContext abilityUsageContext)
    {
        if(previewIndex != -1) 
        {
            abilities[previewIndex].RenderSelectionUI(abilityUsageContext);
        }
    }

    public void StopPreviewing()
    {
        isPreviewingAbility = false;
        if(previewIndex != -1) abilities[previewIndex].DespawnPreview();

        previewIndex = -1;
    }

    public void ConfirmAbility(AbilityUsageContext abilityUsageContext)
    {
        if(previewIndex != -1) 
        {
            if(abilities[previewIndex].Validate(abilityUsageContext)) 
            {
                UseAbility(previewIndex, abilityUsageContext);
                previewIndex = -1;
            }
        }
    }

    public void UseAbility(int index, AbilityUsageContext abilityUsageContext)
    {
        if(IsDead) return;

        if(!isTurn) return;

        if(isUsingAbility) return;
        
        if(index >= abilities.Count) return;
        
        if(isPreviewingAbility) StopPreviewing();

        Debug.Log("CombatUnit: " + UnitName + " tries to use ability " + (index + 1) + " " + abilities[index].AbilityName);

        if(abilities[index].EnergyCost <= CurrentEnergy)
        {
            OnCombatLogChannel.Raise("" + UnitName + " used " + abilities[index].AbilityName);
            
            abilities[index].SpawnAbility(transform, this, abilityUsageContext);
            isUsingAbility = true;
            onUseAbility.Invoke(abilities[index].AbilityName);

            int cost = abilities[index].EnergyCost;
            CurrentEnergy -= cost;
            RefresCanAffordAbilities();
            OnEnergyUpdate_Absolute.Invoke(CurrentEnergy);
            // OnEnergyUpdate_Delta.Invoke(cost * (-1));

            Debug.Log("CombatUnit: " + UnitName + " spends " + abilities[index].EnergyCost + " energy leaving it with " + CurrentEnergy + " energy");
        }
    }

    public void RefresCanAffordAbilities()
    {
        foreach (AbilityData ability in abilities)
        {
            if(CurrentEnergy >= ability.EnergyCost)
            {
                ability.OnUpdateCanAfford.Invoke(true);
            }
            else
            {
                ability.OnUpdateCanAfford.Invoke(false);
            }
        }
    }

    public void OnHurt(DamageResult damageResult)
    {
        if(IsDead) return;

        if(HurtAnimationName != "" && animator != null)animator.Play(HurtAnimationName);

        UpdateEnergyAfterHit(damageResult);

        OnCombatLogChannel.Raise("" + UnitName + " took " + damageResult.trueDamage.Print() + " damage");
    }

    public void OnHeal(DamageResult damageResult)
    {
        if(IsDead) return;
        
        if(HealAnimationName != "" && animator != null)animator.Play(HealAnimationName);

        UpdateEnergyAfterHit(damageResult);

        OnCombatLogChannel.Raise("" + UnitName + " healed " + damageResult.trueDamage.Print());
    }

    private void UpdateEnergyAfterHit(DamageResult damageResult)
    {
        if(damageResult.weakOrRessistant == WeakOrRessistant.WEAK)
        {
            CurrentEnergy -= 1;
            RefresCanAffordAbilities();
            if(CurrentEnergy < 0) CurrentEnergy = 0;
            OnEnergyUpdate_Absolute.Invoke(CurrentEnergy);
            OnEnergyUpdate_Delta.Invoke(-1);
        }
        else if(damageResult.weakOrRessistant == WeakOrRessistant.RESSISTANT)
        {
            // CurrentEnergy += 1 * Stats.data.EnergyGain;
            // RefresCanAffordAbilities();
            // OnEnergyUpdate_Absolute.Invoke(CurrentEnergy);
            // OnEnergyUpdate_Delta.Invoke(1);
        }
        else if(damageResult.weakOrRessistant == WeakOrRessistant.HEALS)
        {
            CurrentEnergy += 1 * Stats.EnergyGain.TrueValue();
            RefresCanAffordAbilities();
            OnEnergyUpdate_Absolute.Invoke(CurrentEnergy);
            OnEnergyUpdate_Delta.Invoke(1);
        }
    }

    public void OnDeath()
    {
        if(DeathAnimationName != "" && animator != null)animator.Play(DeathAnimationName);
        IsDead = true;
    }

    public void OnRevive()
    {
        if(ReviveAnimationName != "" && animator != null)animator.Play(ReviveAnimationName);
    }

    public void DestroyUnit()
    {
        onDestroy.Invoke();
        Destroy(gameObject, 5f);
    }

    IEnumerator onPrompt()
    {
        // waits until next frame to avoid an infinite call stack
        yield return null;

        isTurn = true;

        if(IsDead) EndTurn();
        else
        {
            if(CurrentEnergy < Stats.EnergySoftCap.TrueValue())
            {
                int difference = Stats.EnergySoftCap.TrueValue() - CurrentEnergy;
                CurrentEnergy = Stats.EnergySoftCap.TrueValue();
                
                OnEnergyUpdate_Absolute.Invoke(CurrentEnergy);
                OnEnergyUpdate_Delta.Invoke(difference);
            }

            RefresCanAffordAbilities();

            Debug.Log("CombatUnit: It is the " + UnitName + "s turn\nEnergy: " + CurrentEnergy);

            if(followUnit)
            {
                RequestStartFollowingUnitChannel.Raise(this);
            }
            else
            {
                RequestFocusUnitChannel.Raise(transform.position);
            }

            

            onStartTurn.Invoke();
        }
    }

}

[System.Serializable]
public enum CombatFaction
{
    PLAYER, GOBLIN, ROBOT, ENVIROMENT
}
