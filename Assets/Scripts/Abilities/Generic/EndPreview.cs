using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EndPreview : AbilityPreview
{
    
    public override void Setup(CombatUnit newCaster)
    {
        base.Setup(newCaster);

    }

    protected override void RenderUI()
    {
        UITileMapRequest request = new UITileMapRequest();
        request.header = UITileMapRequestHeader.PLACE;
        request.location = m_location;
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

