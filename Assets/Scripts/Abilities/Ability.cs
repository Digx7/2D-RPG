using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Ability : MonoBehaviour
{
    public string AbilityName;
    public float LifeTime = 1f;
    public string AnimationName;
    public bool ForceEndTurn = false;
    protected CombatUnit m_caster;
    
    public virtual void Use()
    {
        if(AnimationName != "")m_caster.animator.Play(AnimationName);
        StartCoroutine(Timer());
    }

    public virtual void Setup(CombatUnit newCaster)
    {
        m_caster = newCaster;
    }

    public virtual void Teardown()
    {
        if(ForceEndTurn)m_caster.EndTurn();
        else m_caster.EndTurnIfEnergyIsOut();
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(LifeTime);
        Teardown();
        Destroy(gameObject);
    }
}

