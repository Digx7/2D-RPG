using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Goblin_Ability2 : Ability
{
    public Damage HealAmount;
    
    public override void Use()
    {
        Debug.Log("Goblin Uses " + AbilityName);

        Health health = m_caster.GetComponent<Health>();
        health.Heal(HealAmount);

        base.Use();
    }
}