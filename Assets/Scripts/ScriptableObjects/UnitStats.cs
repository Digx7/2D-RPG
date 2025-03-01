using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewUnitStats", menuName = "ScriptableObjects/Combat/Stats", order = 1)]
public class UnitStats : ScriptableObject
{
    public UnitStatsData data;
}

[System.Serializable]
public struct UnitStatsData
{
    public int Speed;
    public int EnergyGain;
}