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
    protected AbilityUsageContext m_context;
    
    public virtual void Use()
    {
        if(AnimationName != "")m_caster.animator.Play(AnimationName);
        StartCoroutine(Timer());
    }

    public virtual void Setup(CombatUnit newCaster, AbilityUsageContext abilityUsageContext)
    {
        m_caster = newCaster;
        m_context = abilityUsageContext;

        Debug.Log("Ability Setup(): context m_mousePos = " + m_context.m_mousePos);
    }

    public virtual void Teardown()
    {
        if(ForceEndTurn)m_caster.EndTurn();
        else m_caster.EndTurnIfEnergyIsOut();
        
        Destroy(gameObject);
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(LifeTime);
        Teardown();
    }
}

[System.Serializable]
public struct AbilityUsageContext
{
    public Vector3 m_mousePos;

    public void Setup()
    {
        m_mousePos = new Vector3();
    }
}