using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AbilityTextHelper : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void RenderAbility(string abilityName)
    {
        textMeshProUGUI.text = abilityName;
    }

    public void Clear()
    {
        textMeshProUGUI.text = "";
    }
}
