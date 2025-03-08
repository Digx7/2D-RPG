using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CombatUnitNotificationElement : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;

    public void SetNotification(CombatUnitNotification notification)
    {
        textMeshProUGUI.text = notification.message;
        textMeshProUGUI.color = notification.color;
        
        float fontSize = 50f;

        switch (notification.size)
        {
            case CombatUnitNotificationSize.SMALL:
                fontSize = 50f;
                break;
            case CombatUnitNotificationSize.MEDIUM:
                fontSize = 75f;
                break;
            case CombatUnitNotificationSize.LARGE:
                fontSize = 100f;
                break;
            default:
                break;
        }

        textMeshProUGUI.fontSize = fontSize;

        Destroy(gameObject, 2f);
    }
}
