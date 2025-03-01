using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GoblinAI : MonoBehaviour
{
    public float thinkTime = 0.5f;
    private CombatUnit combatUnit;

    private void Awake()
    {
        combatUnit = GetComponent<CombatUnit>();
    }

    public void OnTurn()
    {
        StartCoroutine(Think());
    }

    IEnumerator Think()
    {
        AbilityUsageContext abilityUsageContext = new AbilityUsageContext();
        abilityUsageContext.Setup();

        yield return new WaitForSeconds(thinkTime);
        if(combatUnit.CurrentEnergy == 2)
            combatUnit.UseAbility(0, abilityUsageContext);
        else combatUnit.UseAbility(1, abilityUsageContext);
    }
}
