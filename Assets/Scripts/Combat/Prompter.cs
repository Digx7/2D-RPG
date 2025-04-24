using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

public class Prompter : MonoBehaviour
{
    [SerializeField] private Channel onNewRoundStartChannel;
    [SerializeField] private Channel onNewTurnChannel;
    [SerializeField] private SpriteListChannel onCombatIconsUpdate;
    [SerializeField] private CombatUnitListChannel onCombatUnitTurnOrderUpdateChannel;

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
        Debug.Log("Prompter: UpdateCombatUnitList");
        combatUnits = newList;
        List<Sprite> turnIconList = new List<Sprite>();

        foreach (CombatUnit unit in combatUnits)
        {
            Debug.Log("Prompter: added unit " + unit.UnitName);

            turnIconList.Add(unit.TurnOrderIcon);
        }

        onCombatIconsUpdate.Raise(turnIconList);
        onCombatUnitTurnOrderUpdateChannel.Raise(combatUnits);
    }

    public void PromptFirstUnit()
    {
        combatUnits[combatUnitIndex].Prompt();
        combatUnitIndex++;
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
        onNewTurnChannel.Raise();

        // Debug.Log("Prompter: Increased combat unit index to " + combatUnitIndex);
    }



    



}
