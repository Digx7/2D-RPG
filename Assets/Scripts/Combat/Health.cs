using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    public float MaxHealth = 100;
    public float CurrentHealth {get; private set;}
    public List<Modifier> Modifiers;
    public DamageResultEvent OnDamage;
    public DamageResultEvent OnHeal;
    public UnityEvent OnDeath;
    public UnityEvent OnRevive;
    private bool isDead = false;
    private CombatUnit m_CombatUnit;

    private void Awake()
    {
        // CurrentHealth = MaxHealth;


        gameObject.TryGetComponent<CombatUnit>(out m_CombatUnit);
        if(m_CombatUnit != null)
        {
            MaxHealth = m_CombatUnit.Stats.MaxHealth.TrueValue();

            Modifiers = m_CombatUnit.Stats.GetAllDamageModifiers();
        }

        CurrentHealth = MaxHealth;
    }

    public void Damage(Damage incomingDamage)
    {
        DamageResult damageResult = GetTrueDamage(incomingDamage);

        CurrentHealth -= damageResult.trueDamage.amount;
        if(CurrentHealth < 0) CurrentHealth = 0;

        if(damageResult.weakOrRessistant == WeakOrRessistant.HEALS)
        {
            damageResult.trueDamage.amount *= -1;
            OnHeal.Invoke(damageResult);
            if(isDead && CurrentHealth > 0) OnRevive.Invoke();
        }
        else
        {
            OnDamage.Invoke(damageResult);
            if(CheckIfIsDead()) Dead();
        }
    }

    public void Heal(Damage incomingHeal)
    {
        Damage(incomingHeal);
    }

    private DamageResult GetTrueDamage(Damage incomingDamage)
    {
        DamageResult damageResult;

        damageResult.trueDamage = incomingDamage;

        foreach (Modifier modifier in Modifiers)
        {
            if(incomingDamage.damageType == modifier.damageType)
            {
                damageResult.trueDamage.amount = damageResult.trueDamage.amount * modifier.multiplier;
            }
        }

        if(damageResult.trueDamage.amount > incomingDamage.amount) 
        {
            damageResult.weakOrRessistant = WeakOrRessistant.WEAK;
        }
        else if(damageResult.trueDamage.amount < incomingDamage.amount && damageResult.trueDamage.amount > 0) 
        {
            damageResult.weakOrRessistant = WeakOrRessistant.RESSISTANT;
        }
        else if(damageResult.trueDamage.amount < 0)
        {
            damageResult.weakOrRessistant = WeakOrRessistant.HEALS;
        }
        else 
        {
            damageResult.weakOrRessistant = WeakOrRessistant.NORMAL;
        }

        return damageResult;
    }

    public bool CheckIfIsDead()
    {
        if(CurrentHealth <= 0) return true;
        else return false;
    }

    public void Dead()
    {
        isDead = true;
        OnDeath.Invoke();
    }
}

[System.Serializable]
public struct Damage
{
    public DamageType damageType;
    public float amount;
    public string Print()
    {
        return amount + " " + damageType;
    }
}

[System.Serializable]
public struct DamageResult
{
    public WeakOrRessistant weakOrRessistant;
    public Damage trueDamage;
}

[System.Serializable]
public struct Modifier
{
    public DamageType damageType;
    public float multiplier;
}

[System.Serializable]
public enum DamageType
{
    SLASH, PIERCE, BLUDGEON, FIRE, EARTH, AIR, WATER, LIFE, LIGHT, DARK
}

[System.Serializable]
public enum WeakOrRessistant
{
    WEAK, RESSISTANT, HEALS, NORMAL
}

[System.Serializable]
public class DamageEvent : UnityEvent<Damage> {}

[System.Serializable]
public class DamageResultEvent : UnityEvent<DamageResult> {}