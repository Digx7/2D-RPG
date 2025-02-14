using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ActiveTimeLoreMenu : UIMenu
{
    public GameObject profilePrefab;
    public Transform profileSpawnPoint;
    public GameObject cardPrefab;
    public Transform cardSpawnPoint;
    
    // References
    private ActiveTimeLoreManager activeTimeLoreManager;
    private List<ActiveTimeLore> relevantActiveTimeLore;
    private ActiveTimeLoreCard activeCard;
    
    public override void Setup(UIWidgetData newUIWidgetData)
    {
        activeTimeLoreManager = GameObject.FindFirstObjectByType<ActiveTimeLoreManager>();

        if(activeTimeLoreManager != null)
        {
            relevantActiveTimeLore = activeTimeLoreManager.relevantActiveTimeLore;
            SpawnAllProfileIcons();
        }
        
        base.Setup(newUIWidgetData);
    }

    public override void Teardown()
    {
        base.Teardown();
    }

    public void OnSelectProfileIcon(ActiveTimeLore chosenTimeLore)
    {
        foreach (Transform child in profileSpawnPoint)
        {
            Destroy(child.gameObject);
        }
        SpawnCard(chosenTimeLore);
    }

    public void OnReturnFromCard()
    {
        foreach (Transform child in cardSpawnPoint)
        {
            Destroy(child.gameObject);
        }
        SpawnAllProfileIcons();
    }

    public void OnBack()
    {
        UnloadSelf();
    }

    private void SpawnAllProfileIcons()
    {
        for (int i = 0; i < relevantActiveTimeLore.Count; i++)
        {
            SpawnProfileIcon(relevantActiveTimeLore[i]);
        }
    }

    private void SpawnProfileIcon(ActiveTimeLore activeTimeLore)
    {
        GameObject obj = Instantiate(profilePrefab, profileSpawnPoint);
        ActiveTimeLoreProfileIcon activeTimeLoreProfileIcon = obj.GetComponent<ActiveTimeLoreProfileIcon>();

        activeTimeLoreProfileIcon.OnSelect.AddListener(OnSelectProfileIcon);
        activeTimeLoreProfileIcon.Render(activeTimeLore);
    }
    
    private void SpawnCard(ActiveTimeLore activeTimeLore)
    {
        GameObject obj = Instantiate(cardPrefab, cardSpawnPoint);
        activeCard = obj.GetComponent<ActiveTimeLoreCard>();

        activeCard.OnReturn.AddListener(OnReturnFromCard);
        activeCard.Render(activeTimeLore);
    }
}
