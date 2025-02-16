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

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void Damage(Damage incomingDamage)
    {
        DamageResult damageResult;

        damageResult.trueDamage = incomingDamage.amount;

        foreach (Modifier modifier in Modifiers)
        {
            if(incomingDamage.damageType == modifier.damageType)
            {
                damageResult.trueDamage = damageResult.trueDamage * modifier.multiplier;
            }
        }

        CurrentHealth -= damageResult.trueDamage;

        if(damageResult.trueDamage > incomingDamage.amount) 
        {
            damageResult.weakOrRessistant = WeakOrRessistant.WEAK;

            Debug.Log("Health: took " + damageResult.trueDamage + " " + incomingDamage.damageType + " damage\nIts super effective\nCurrentHealth: " + CurrentHealth);
        }
        else if(damageResult.trueDamage > incomingDamage.amount) 
        {
            damageResult.weakOrRessistant = WeakOrRessistant.RESSISTANT;

            Debug.Log("Health: took " + damageResult.trueDamage + " " + incomingDamage.damageType + " damage\nIts NOT effective\nCurrentHealth: " + CurrentHealth);
        }
        else 
        {
            damageResult.weakOrRessistant = WeakOrRessistant.NORMAL;

            Debug.Log("Health: took " + damageResult.trueDamage + " " + incomingDamage.damageType + " damage\nIts normal\nCurrentHealth: " + CurrentHealth);
        }

        OnDamage.Invoke(damageResult);

        if(CheckIfIsDead()) Dead();
    }

    public void Heal(Damage incomingHeal)
    {
        DamageResult healResult;

        healResult.trueDamage = incomingHeal.amount;

        foreach (Modifier modifier in Modifiers)
        {
            if(incomingHeal.damageType == modifier.damageType)
            {
                healResult.trueDamage = healResult.trueDamage * modifier.multiplier;
            }
        }

        CurrentHealth += healResult.trueDamage;

        if(healResult.trueDamage > incomingHeal.amount) healResult.weakOrRessistant = WeakOrRessistant.WEAK;
        else if(healResult.trueDamage > incomingHeal.amount) healResult.weakOrRessistant = WeakOrRessistant.RESSISTANT;
        else healResult.weakOrRessistant = WeakOrRessistant.NORMAL;

        Debug.Log("Health: Healed " + healResult.trueDamage + "\nCurrentHealth: " + CurrentHealth);

        OnHeal.Invoke(healResult);

        if(isDead && CurrentHealth > 0) OnRevive.Invoke();
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
}

[System.Serializable]
public struct DamageResult
{
    public WeakOrRessistant weakOrRessistant;
    public float trueDamage;
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
    FIRE, EARTH, AIR, WATER, LIGHT, DARK
}

[System.Serializable]
public enum WeakOrRessistant
{
    WEAK, RESSISTANT, NORMAL
}

[System.Serializable]
public class DamageEvent : UnityEvent<Damage> {}

[System.Serializable]
public class DamageResultEvent : UnityEvent<DamageResult> {}