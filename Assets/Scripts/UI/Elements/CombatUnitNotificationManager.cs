using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CombatUnitNotificationManager : MonoBehaviour
{
    public GameObject notificationPrefab;
    public GameObject notificationParent;

    private Queue<CombatUnitNotification> notificationQueue;
    private bool notificationIsBeingRendeered = false;


    public void Awake()
    {
        notificationQueue = new Queue<CombatUnitNotification>();
    }

    public void Update()
    {
        if(!notificationIsBeingRendeered && notificationQueue.Count > 0)
        {
            notificationIsBeingRendeered = true;
            StartCoroutine(Timer());
            SpawnNotification(notificationQueue.Dequeue());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(2.1f);
        notificationIsBeingRendeered = false;
    }

    public void Notify(CombatUnitNotification notification)
    {
        notificationQueue.Enqueue(notification);
    }

    private void SpawnNotification(CombatUnitNotification notification)
    {
        GameObject obj = Instantiate(notificationPrefab, notificationParent.transform);
        CombatUnitNotificationElement combatUnitNotificationElement = obj.GetComponent<CombatUnitNotificationElement>();
        combatUnitNotificationElement.SetNotification(notification);
    }

    public void OnDamage(DamageResult damageResult)
    {
        CombatUnitNotification notification;

        notification.message = damageResult.trueDamage.ToString();
        Color color = Color.white;
        CombatUnitNotificationSize size = CombatUnitNotificationSize.MEDIUM;

        switch (damageResult.weakOrRessistant)
        {
            case WeakOrRessistant.WEAK:
                color = Color.red;
                size = CombatUnitNotificationSize.LARGE;
                break;
            case WeakOrRessistant.NORMAL:
                color = Color.white;
                size = CombatUnitNotificationSize.MEDIUM;
                break;
            case WeakOrRessistant.RESSISTANT:
                color = Color.blue;
                size = CombatUnitNotificationSize.SMALL;
                break;
            default:
                break;
        }

        notification.color = color;
        notification.size = size;

        Notify(notification);
    }

    public void OnHeal(DamageResult healResult)
    {
        CombatUnitNotification notification;

        notification.message = healResult.trueDamage.ToString();
        Color color = Color.green;
        CombatUnitNotificationSize size = CombatUnitNotificationSize.MEDIUM;

        switch (healResult.weakOrRessistant)
        {
            case WeakOrRessistant.WEAK:
                size = CombatUnitNotificationSize.LARGE;
                break;
            case WeakOrRessistant.NORMAL:
                size = CombatUnitNotificationSize.MEDIUM;
                break;
            case WeakOrRessistant.RESSISTANT:
                size = CombatUnitNotificationSize.SMALL;
                break;
            default:
                break;
        }

        notification.color = color;
        notification.size = size;

        Notify(notification);
    }

    public void OnEnergyChange(int delta)
    {
        CombatUnitNotification notification;

        if(delta > 0)
        {
            notification.message = "+" + delta.ToString();
        }
        else
        {
            notification.message = delta.ToString();
        }

        notification.color = new Color(1, 0.3882f, 0, 1);
        notification.size = CombatUnitNotificationSize.MEDIUM;

        Notify(notification);
    }
}

public struct CombatUnitNotification
{
    public string message;
    public Color color;
    public CombatUnitNotificationSize size;
}

[System.Serializable]
public enum CombatUnitNotificationSize {SMALL, MEDIUM, LARGE}

[System.Serializable]
public class CombatUnitNotificationEvent : UnityEvent<CombatUnitNotification> {}
