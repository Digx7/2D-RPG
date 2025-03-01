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
    public UnityEvent onEndTurn;
    public UnityEvent onDeath;
    public UnityEvent onDestroy;
    public IntEvent OnEnergyUpdate;

    private bool isTurn = false;
    private bool isUsingAbility = false;
    private bool isPreviewingAbility = false;
    private int previewIndex = -1;
    // private var abilityContext = null;
    public bool IsDead {get; private set;} = false;
    // public bool IsWeak {get; private set;} = false;
    // public void SetWeak(){IsWeak = true;}
    // public bool IsStrong {get; private set;} = false;
    // public void SetStrong(){IsStrong = true;}

    private void OnEnable()
    {
        CurrentEnergy = DefaultEnergy;
        OnEnergyUpdate.Invoke(CurrentEnergy);
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
    }

    public void EndTurn()
    {
        if(!isTurn) return;

        Debug.Log("CombatUnit: " + UnitName + " ending turn");

        // IsWeak = false;
        // IsStrong = false;
        CurrentEnergy = DefaultEnergy;
        OnEnergyUpdate.Invoke(CurrentEnergy);

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
        
        if(isPreviewingAbility) return;

        Debug.Log("CombatUnit: " + UnitName + " is previewing ability " + (index + 1) + " " + abilities[index].AbilityName);

        abilities[index].SpawnPreview(transform, this);

        isPreviewingAbility = true;
        previewIndex = index;
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
            abilities[index].SpawnAbility(transform, this, abilityUsageContext);
            // abilities[index].SpawnAbility(transform, this, abilityContext);
            isUsingAbility = true;

            CurrentEnergy -= abilities[index].EnergyCost;
            OnEnergyUpdate.Invoke(CurrentEnergy);

            Debug.Log("CombatUnit: " + UnitName + " spends " + abilities[index].EnergyCost + " energy leaving it with " + CurrentEnergy + " energy");
        }
    }

    public void OnHurt(DamageResult damageResult)
    {
        if(IsDead) return;

        if(HurtAnimationName != "" && animator != null)animator.Play(HurtAnimationName);

        if(damageResult.weakOrRessistant == WeakOrRessistant.WEAK)
        {
            CurrentEnergy -= 1;
            if(CurrentEnergy < 0) CurrentEnergy = 0;
            OnEnergyUpdate.Invoke(CurrentEnergy);
        }
        else if(damageResult.weakOrRessistant == WeakOrRessistant.RESSISTANT)
        {
            CurrentEnergy += 1;
            OnEnergyUpdate.Invoke(CurrentEnergy);
        }
    }

    public void OnHeal(DamageResult damageResult)
    {
        if(IsDead) return;
        
        if(HealAnimationName != "" && animator != null)animator.Play(HealAnimationName);
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
            // CurrentEnergy = DefaultEnergy;
            // if(IsWeak) CurrentEnergy -= 1;
            // if(IsStrong) CurrentEnergy += 1;
            
            Debug.Log("CombatUnit: It is the " + UnitName + "s turn\nEnergy: " + CurrentEnergy);

            onStartTurn.Invoke();
        }
    }

}

[System.Serializable]
public enum CombatFaction
{
    PLAYER, GOBLIN, ROBOT, ENVIROMENT
}
