using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DamageTextHelper : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void RenderDamage(DamageResult damageResult)
    {
        StartCoroutine(DamageAnimation(damageResult));
    }

    public void RenderHeal(DamageResult damageResult)
    {
        StartCoroutine(HealAnimation(damageResult));
    }

    IEnumerator DamageAnimation(DamageResult damageResult)
    {
        switch (damageResult.weakOrRessistant)
        {
            case WeakOrRessistant.WEAK:
                textMeshProUGUI.color = Color.yellow;
                break;
            case WeakOrRessistant.RESSISTANT:
                textMeshProUGUI.color = Color.blue;
                break;
            case WeakOrRessistant.NORMAL:
                textMeshProUGUI.color = Color.white;
                break;
            default:
                textMeshProUGUI.color = Color.white;
                break;
        }
        textMeshProUGUI.text = "" + damageResult.trueDamage;
        yield return new WaitForSeconds(2f);
        textMeshProUGUI.text = "";
    }

    IEnumerator HealAnimation(DamageResult damageResult)
    {
        textMeshProUGUI.color = Color.green;
        textMeshProUGUI.text = "" + damageResult.trueDamage;
        yield return new WaitForSeconds(2f);
        textMeshProUGUI.text = "";
    }
}
