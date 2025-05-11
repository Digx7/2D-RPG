using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;


public class StringTableEntryChanger : MonoBehaviour
{
    public LocalizeStringEvent localizeStringEvent;

    public LocalizedString nextStringReference;

    public void OnClick()
    {
        localizeStringEvent.StringReference = nextStringReference;
    }
}