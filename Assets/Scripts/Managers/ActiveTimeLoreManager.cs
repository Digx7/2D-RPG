using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

public class ActiveTimeLoreManager : Singleton<ActiveTimeLoreManager>
{
    public List<ActiveTimeLore> relevantActiveTimeLore;

    [SerializeField] private ActiveTimeLoreChannel RequestAddRelevantLoreChannel;
    [SerializeField] private ActiveTimeLoreChannel RequestRemoveRelevantLoreChannel;

    private void OnEnable()
    {
        RequestAddRelevantLoreChannel.channelEvent.AddListener(AddRelevantLore);
        RequestRemoveRelevantLoreChannel.channelEvent.AddListener(RemoveRelevantLore);
    }

    private void OnDisable()
    {
        RequestAddRelevantLoreChannel.channelEvent.RemoveListener(AddRelevantLore);
        RequestRemoveRelevantLoreChannel.channelEvent.RemoveListener(RemoveRelevantLore);
    }

    public void AddRelevantLore(ActiveTimeLore relevantLore)
    {
        if(relevantActiveTimeLore.Contains(relevantLore)) return;

        relevantActiveTimeLore.Add(relevantLore);
    }

    public void RemoveRelevantLore(ActiveTimeLore relevantLore)
    {
        relevantActiveTimeLore.Remove(relevantLore);
    }

}
