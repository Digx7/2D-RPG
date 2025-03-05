using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HealPreview : AbilityPreview
{
    private int m_range = 3;
    
    public override void Setup(CombatUnit newCaster)
    {
        base.Setup(newCaster);
    }

    protected override void RenderUI()
    {
        UITileMapRequest request = new UITileMapRequest();
        request.header = UITileMapRequestHeader.FILL;
        request.location = m_location;
        request.range = 3;
        request.context = selectedContext;

        requestUITileMapChannel.Raise(request);
    }

    protected override void ClearUI()
    {
        UITileMapRequest request = new UITileMapRequest();
        request.header = UITileMapRequestHeader.CLEAR;
        request.context = selectedContext;

        requestUITileMapChannel.Raise(request);
    }
}

