using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UICharacterHotBarWidget : UIWidget
{

    public GameObject actionElementPrefab;
    public Transform actionElementParent;
    
    public override void Setup(UIWidgetData newUIWidgetData)
    {
        base.Setup(newUIWidgetData);

    }

    public override void Teardown()
    {
        
        base.Teardown();
    }

    public void SpawnActionElement(Sprite sprite, string button, string cost)
    {
        GameObject obj = Instantiate(actionElementPrefab, actionElementParent);
        CharacterHotBarElement characterHotBarElement = obj.GetComponent<CharacterHotBarElement>();

        characterHotBarElement.SetImage(sprite);
        characterHotBarElement.SetButton(button);
        characterHotBarElement.SetCost(cost);
    }
}
