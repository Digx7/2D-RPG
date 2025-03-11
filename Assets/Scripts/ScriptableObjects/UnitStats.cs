using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewUnitStats", menuName = "ScriptableObjects/Combat/Stats", order = 1)]
public class UnitStats : ScriptableObject
{
    // public UnitStatsData data;
    public Stat Speed;
    public Stat MaxHealth;

    public List<DamageType> Weaknesses;
    public List<DamageType> Resistances;
    public List<DamageType> Healed;

    public List<Modifier> GetAllDamageModifiers()
    {
        List<Modifier> output = new List<Modifier>();
        
        foreach (DamageType type in Weaknesses)
        {
            Modifier modifier;
            modifier.damageType = type;
            modifier.multiplier = 2;

            output.Add(modifier);
        }

        foreach (DamageType type in Resistances)
        {
            Modifier modifier;
            modifier.damageType = type;
            modifier.multiplier = 0.5f;

            output.Add(modifier);
        }

        foreach (DamageType type in Healed)
        {
            Modifier modifier;
            modifier.damageType = type;
            modifier.multiplier = -1;

            output.Add(modifier);
        }

        return output;
    }

    public Stat EnergyGain;
    public Stat EnergySoftCap;
    public Stat EnergyHardCap;

    public Stat Strength;
    public Stat Dexterity;
    public Stat Intelligence;
    public Stat Wisdom;
    public Stat Charisma;
}

[System.Serializable]
public struct UnitStatsData
{
    public int Speed;
    public int EnergyGain;
}

[System.Serializable]
public struct Stat
{
    public int baseValue;

    [HideInInspector]
    public int modifiers;
    public int TrueValue() {return baseValue + modifiers;}
}