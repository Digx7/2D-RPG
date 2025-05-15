using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class OneWayPlatform : MonoBehaviour
{
    [SerializeField] private BoxCollider2D collider2D;
    [SerializeField] private Channel onTryDropChannel;

    public void OnEnable()
    {
        onTryDropChannel.channelEvent.AddListener(OnTryDrop);
    }

    public void OnDisable()
    {
        onTryDropChannel.channelEvent.RemoveListener(OnTryDrop);
    }

    public void OnTryDrop()
    {
        collider2D.excludeLayers = LayerMask.GetMask("Player");
    }

    public void OnPlayerEnter()
    {
        collider2D.excludeLayers = 0;
    }

    public void OnPlayerLeave()
    {
        collider2D.excludeLayers = LayerMask.GetMask("Player");
    }
}