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
        yield return new WaitForSeconds(thinkTime);
        Debug.Log("GoblinAI: Using ability 1 on gobins turn");
        combatUnit.UseAbility(0);
    }
}
