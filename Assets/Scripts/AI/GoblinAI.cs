using UnityEngine;

public class GoblinAI : MonoBehaviour
{
    private CombatUnit combatUnit;

    private void Awake()
    {
        combatUnit = GetComponent<CombatUnit>();
    }

    public void OnTurn()
    {
        Debug.Log("GoblinAI: Using ability 1 on gobins turn");
        combatUnit.UseAbility(0);
    }
}
