using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;

public class CombatUnit : MonoBehaviour
{
    public string UnitName;
    public List<AbilityData> abilities;
    public CombatFaction CombatFaction;
    public Animator animator;
    public string HurtAnimationName;
    public string HealAnimationName;
    public string DeathAnimationName;
    public string ReviveAnimationName;
    public Channel onEndUnitTurnChannel;
    public UnityEvent onStartTurn;
    public UnityEvent onEndTurn;
    public UnityEvent onDeath;

    private bool isTurn = false;
    public bool IsDead {get; private set;} = false;

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

    public void EndTurn()
    {
        if(!isTurn) return;

        Debug.Log("CombatUnit: " + UnitName + " ending turn");

        isTurn = false;
        onEndUnitTurnChannel.Raise();
        onEndTurn.Invoke();
    }

    public void UseAbility(int index)
    {
        if(IsDead) return;
        
        if(index >= abilities.Count) return;

        Debug.Log("CombatUnit: " + UnitName + " uses ability " + (index + 1));

        abilities[index].SpawnAbility(transform, this);
    }

    public void OnHurt()
    {
        if(HurtAnimationName != "")animator.Play(HurtAnimationName);
    }

    public void OnHeal()
    {
        if(HealAnimationName != "")animator.Play(HealAnimationName);
    }

    public void OnDeath()
    {
        if(DeathAnimationName != "")animator.Play(DeathAnimationName);
        IsDead = true;
    }

    public void OnRevive()
    {
        if(ReviveAnimationName != "")animator.Play(ReviveAnimationName);
    }

    public void DestroyUnit()
    {
        Destroy(gameObject, 5f);
    }

    IEnumerator onPrompt()
    {
        // waits until next frame to avoid an infinite call stack
        yield return null;
        
        Debug.Log("CombatUnit: It is the " + UnitName + "s turn");

        if(IsDead) EndTurn();
        else
        {
            isTurn = true;
            onStartTurn.Invoke();
        }
    }

}

[System.Serializable]
public enum CombatFaction
{
    PLAYER, GOBLIN, ROBOT
}
