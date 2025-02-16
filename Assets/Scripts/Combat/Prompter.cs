using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

public class Prompter : MonoBehaviour
{
    [SerializeField] private Channel onNewRoundStartChannel;

    private List<CombatUnit> combatUnits;
    private int combatUnitIndex = 0;

    private void Awake()
    {
        combatUnits = new List<CombatUnit>();
    }

    private void OnEnable()
    { 
    }

    private void OnDisable()
    {
    }

    public void UpdateCombatUnitList(List<CombatUnit> newList) 
    {
        combatUnits = newList;

        foreach (CombatUnit unit in combatUnits)
        {
            Debug.Log("Prompter: added unit " + unit.UnitName);
        }
    }

    public void PromptNextUnit()
    {
        if(combatUnitIndex >= combatUnits.Count)
        {
            combatUnitIndex = 0;
            // Debug.Log("Prompter: Should start new combat round");
            onNewRoundStartChannel.Raise();
        }
        
        // Debug.Log("Prompter: Prompting next unit at index " + combatUnitIndex);

        combatUnits[combatUnitIndex].Prompt();
        combatUnitIndex++;

        // Debug.Log("Prompter: Increased combat unit index to " + combatUnitIndex);
    }



    



}
