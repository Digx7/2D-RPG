using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

public class CombatManager : Singleton<CombatManager>
{
    [SerializeField] private Channel requestCombatStartChannel;
    [SerializeField] private Channel onCombatStartChannel;
    [SerializeField] private Channel onCombatEndChannel;
    [SerializeField] private Channel onEndUnitTurnChannel;
    [SerializeField] private Channel onNewRoundStartChannel;
    public UnityEvent onCombatStart;
    public UnityEvent onCombatEnd;
    private Prompter prompter;
    private int roundNumber;

    private List<CombatUnit> combatUnits;

    public override void Awake()
    {
        base.Awake();

        prompter = gameObject.GetComponent<Prompter>();
        combatUnits = new List<CombatUnit>();
    }

    private void OnEnable()
    {
        requestCombatStartChannel.channelEvent.AddListener(StartCombat);
        onEndUnitTurnChannel.channelEvent.AddListener(OnEndUnitTurn);
        onNewRoundStartChannel.channelEvent.AddListener(OnNewRoundStart);
    }

    private void OnDisable()
    {
        requestCombatStartChannel.channelEvent.RemoveListener(StartCombat);
        onEndUnitTurnChannel.channelEvent.RemoveListener(OnEndUnitTurn);
        onNewRoundStartChannel.channelEvent.RemoveListener(OnNewRoundStart);
    }

    private void StartCombat()
    {
        Debug.Log("CombatManager: Starting Combat");
        
        SetupAllCombatUnits();
        prompter.UpdateCombatUnitList(combatUnits);
        roundNumber = 1;

        prompter.PromptNextUnit();

        onCombatStartChannel.Raise();
        onCombatStart.Invoke();
    }

    private void EndCombat()
    {
        Debug.Log("CombatManager: Ending Combat");
        
        foreach (CombatUnit unit in combatUnits)
        {
            if(unit.IsDead) unit.DestroyUnit();
        }

        onCombatEndChannel.Raise();
        onCombatEnd.Invoke();
    }

    private void SetupAllCombatUnits()
    {
        combatUnits.Clear();
        combatUnits.AddRange(FindObjectsByType<CombatUnit>(FindObjectsSortMode.None));
    }

    public void OnEndUnitTurn()
    {
        Debug.Log("CombatManger: A Unit has ended its turn");
        
        if(isBattleOver())
        {
            EndCombat();
            return;
        }

        prompter.PromptNextUnit();
    }

    public void OnNewRoundStart()
    {
        Debug.Log("CombatManager: Starting a new round");
        roundNumber++;
        // EndCombat();
    }

    public bool isBattleOver()
    {
        // TODO trigger if only one faction is left standing

        foreach (CombatUnit unit in combatUnits)
        {
            if(unit.IsDead) return true;
        }

        return false;
    }

}
