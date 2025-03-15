using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : Singleton<CombatManager>
{
    // [SerializeField] private Channel requestCombatStartChannel;
    [SerializeField] private CombatInstanceChannel notifyActiveCombatInstance;
    [SerializeField] private Channel onCombatStartChannel;
    [SerializeField] private Channel onCombatEndChannel;
    [SerializeField] private Channel onEndUnitTurnChannel;
    [SerializeField] private Channel onNewRoundStartChannel;
    [SerializeField] private UIWidgetDataChannel onRequestLoadUIChannel;
    [SerializeField] private UIWidgetDataChannel onRequestUnloadUIChannel;
    [SerializeField] private List<UIWidgetData> combatWidgetDatas;
    public UnityEvent onCombatStart;
    public UnityEvent onCombatEnd;
    private Prompter prompter;
    private int roundNumber;
    private bool allCombatUnitsAreSetup = false;
    private CombatInstance combatInstance;
    private PartyManager partyManager;

    private List<CombatUnit> combatUnits;

    public override void Awake()
    {
        base.Awake();

        prompter = gameObject.GetComponent<Prompter>();
        combatUnits = new List<CombatUnit>();
    }

    private void OnEnable()
    {
        // requestCombatStartChannel.channelEvent.AddListener(StartCombat);
        notifyActiveCombatInstance.channelEvent.AddListener(StartCombat);
        onEndUnitTurnChannel.channelEvent.AddListener(OnEndUnitTurn);
        onNewRoundStartChannel.channelEvent.AddListener(OnNewRoundStart);
    }

    private void OnDisable()
    {
        // requestCombatStartChannel.channelEvent.RemoveListener(StartCombat);
        notifyActiveCombatInstance.channelEvent.RemoveListener(StartCombat);
        onEndUnitTurnChannel.channelEvent.RemoveListener(OnEndUnitTurn);
        onNewRoundStartChannel.channelEvent.RemoveListener(OnNewRoundStart);
    }

    private void StartCombat(CombatInstance instance)
    {
        Debug.Log("CombatManager: Starting Combat");
        
        // loads combat ui
        // if(combatWidgetDatas.Count > 0)
        // {
        //     foreach (UIWidgetData data in combatWidgetDatas)
        //     {
        //         onRequestLoadUIChannel.Raise(data);
        //     }
        // }

        // SetupAllCombatUnits();
        // prompter.UpdateCombatUnitList(combatUnits);
        // roundNumber = 1;

        // prompter.PromptFirstUnit();

        // onCombatStartChannel.Raise();
        // onCombatStart.Invoke();

        combatInstance = instance;

        StartCoroutine(SetupCombat());
    }

    private void EndCombat()
    {
        Debug.Log("CombatManager: Ending Combat");
        
        combatInstance.SetInstanceActive(false);

        // loads combat ui
        if(combatWidgetDatas.Count > 0)
        {
            foreach (UIWidgetData data in combatWidgetDatas)
            {
                onRequestUnloadUIChannel.Raise(data);
            }
        }

        foreach (CombatUnit unit in combatUnits)
        {
            if(unit.IsDead) unit.DestroyUnit();
        }

        partyManager.RemoveAllPartyMembers();

        combatUnits.Clear();

        onCombatEndChannel.Raise();
        onCombatEnd.Invoke();
    }

    // private void SetupAllCombatUnits()
    // {
    //     combatUnits.Clear();
    //     combatUnits.AddRange(FindObjectsByType<CombatUnit>(FindObjectsSortMode.None));
    // }

    public void OnEndUnitTurn()
    {
        Debug.Log("CombatManger: A Unit has ended its turn");
        
        StartCoroutine(EndUnitTurn());
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

        if(IsPlayerFactionDead()) 
        {
            Debug.Log("CombatManager: Player Faction is the only one Left");
            return true;
        }

        if(OnlyOneFactionLives()) return true;
        else return false;
    }

    private bool OnlyOneFactionLives()
    {
        List<CombatFaction> livingFactions = new List<CombatFaction>();

        foreach (CombatUnit unit in combatUnits)
        {
            if(!unit.IsDead && !livingFactions.Contains(unit.combatFaction))
            {
                livingFactions.Add(unit.combatFaction);
            }
        }

        if(livingFactions.Count <= 1) 
        {
            Debug.Log("CombatManager: Only the faction " + livingFactions[0] + " remains\n" + "Number of living factions " + livingFactions.Count);
            
            return true;
        }
        else return false;
    }

    private bool IsPlayerFactionDead()
    {
        return IsFactionDead(CombatFaction.PLAYER);
    }

    private bool IsFactionDead(CombatFaction factionToCheck)
    {
        foreach (CombatUnit unit in combatUnits)
        {
            if(unit.combatFaction == factionToCheck && unit.IsDead == false) return false;
        }
        return true;
    }

    IEnumerator SetupCombat()
    {
        // Gets CombatInstance
        // CombatInstance[] allCombatInstances = GameObject.FindObjectsByType<CombatInstance>(FindObjectsSortMode.None);
        // foreach (CombatInstance instance in allCombatInstances)
        // {
        //     if(instance.isActive)
        //     {
        //         combatInstance = instance;
        //         break;
        //     }
        // }

        if(combatInstance == null)
        {
            Debug.LogError("CombatManager: Combat was triggered but no valid CombatInstance was found");
        }
        
        // Loads UI
        if(combatWidgetDatas.Count > 0)
        {
            foreach (UIWidgetData data in combatWidgetDatas)
            {
                onRequestLoadUIChannel.Raise(data);
            }
        }

        StartCoroutine(SetupAllCombatUnits());
        while (!allCombatUnitsAreSetup)
        {
            yield return null;
        }

        prompter.UpdateCombatUnitList(combatUnits);
        roundNumber = 1;

        prompter.PromptFirstUnit();

        onCombatStartChannel.Raise();
        onCombatStart.Invoke();
    }

    IEnumerator SetupAllCombatUnits()
    {
        combatUnits.Clear();

        // Get Player
        GameObject playerObj = GameObject.FindWithTag("Player");
        CombatUnit player = playerObj.GetComponent<CombatUnit>();
        combatUnits.Add(player);
        
        // Get Party Members
        partyManager = GameObject.FindFirstObjectByType<PartyManager>();
        combatUnits.AddRange(partyManager.PlacePartyMembers(combatInstance.partyMemberSpawnPoints));

        // Get Enemies
        combatUnits.AddRange(combatInstance.allEnemies);
        
        allCombatUnitsAreSetup = true;
        yield return null;
    }

    IEnumerator EndUnitTurn()
    {
        yield return null;
        
        if(isBattleOver())
        {
            EndCombat();
            yield break;
        }

        prompter.PromptNextUnit();
    }

}
