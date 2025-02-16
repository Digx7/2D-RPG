using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;

public class CombatUnit : MonoBehaviour
{
    public string UnitName;
    public Channel onEndUnitTurnChannel;
    public UnityEvent onStartTurn;
    public UnityEvent onEndTurn;

    private bool isTurn = false;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void Prompt()
    {
        // Done to avoid an infinite call stack
        StartCoroutine(onPrompt());
    }

    public void EndTurn()
    {
        if(!isTurn) return;

        Debug.Log("CombatUnit: " + UnitName + " ending turn");

        isTurn = false;
        onEndUnitTurnChannel.Raise();
        onEndTurn.Invoke();
    }

    public void UseAbility1()
    {
        if(!isTurn) return;
        
        Debug.Log("CombatUnit: " + UnitName + " uses ability 1");

        EndTurn();
    }

    public void UseAbility2()
    {
        if(!isTurn) return;
        
        Debug.Log("CombatUnit: " + UnitName + " uses ability 1");

        EndTurn();
    }

    IEnumerator onPrompt()
    {
        // waits until next frame to avoid an infinite call stack
        yield return null;
        
        Debug.Log("CombatUnit: It is the " + UnitName + "s turn");

        isTurn = true;
        onStartTurn.Invoke();
    }

}
