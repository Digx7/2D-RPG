using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AbilityPreview : MonoBehaviour
{
    protected CombatUnit m_caster;
    
    public virtual void Setup(CombatUnit newCaster)
    {
        m_caster = newCaster;
        RenderUI();
    }

    public virtual void Use()
    {
        RenderUI();
    }

    protected virtual void RenderUI() {}

    protected virtual void ClearUI() {}

    public virtual void Teardown()
    {
        ClearUI();
        Destroy(gameObject);
    }
}

