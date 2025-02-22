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
    public int MaxEnergy = 2;
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

    private bool isTurn = false;
    private bool isUsingAbility = false;
    public bool IsDead {get; private set;} = false;
    public bool IsWeak {get; private set;} = false;
    public void SetWeak(){IsWeak = true;}
    public bool IsStrong {get; private set;} = false;
    public void SetStrong(){IsStrong = true;}

    private void OnEnable()
    {
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

        IsWeak = false;
        IsStrong = false;

        isTurn = false;
        isUsingAbility = false;
        onEndUnitTurnChannel.Raise();
        onEndTurn.Invoke();
    }

    public void UseAbility(int index)
    {
        if(IsDead) return;

        if(!isTurn) return;

        if(isUsingAbility) return;
        
        if(index >= abilities.Count) return;

        Debug.Log("CombatUnit: " + UnitName + " tries to use ability " + (index + 1) + " " + abilities[index].AbilityName);

        if(abilities[index].EnergyCost <= CurrentEnergy)
        {
            abilities[index].SpawnAbility(transform, this);
            isUsingAbility = true;

            CurrentEnergy -= abilities[index].EnergyCost;

            Debug.Log("CombatUnit: " + UnitName + " spends " + abilities[index].EnergyCost + " energy leaving it with " + CurrentEnergy + " energy");
        }
    }

    public void OnHurt()
    {
        if(IsDead) return;

        if(HurtAnimationName != "" && animator != null)animator.Play(HurtAnimationName);
    }

    public void OnHeal()
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
            CurrentEnergy = MaxEnergy;
            if(IsWeak) CurrentEnergy -= 1;
            if(IsStrong) CurrentEnergy += 1;
            
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
