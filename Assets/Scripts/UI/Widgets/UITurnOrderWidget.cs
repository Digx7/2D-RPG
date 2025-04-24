using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UITurnOrderWidget : UIWidget
{
    public GameObject iconPrefab;
    public GameObject iconParent;
    public CombatUnitListChannel onSetupChannel;
    public Channel onNextTurnChannel;

    // private List<Sprite> iconList;
    private List<CombatUnit> unitList;
    private int indexStart = 0;

    public override void Setup(UIWidgetData newUIWidgetData)
    {
        base.Setup(newUIWidgetData);

        onSetupChannel.channelEvent.AddListener(SetIcons);
        onNextTurnChannel.channelEvent.AddListener(NextTurn);
    }

    public override void Teardown()
    {
        onSetupChannel.channelEvent.RemoveListener(SetIcons);
        onNextTurnChannel.channelEvent.RemoveListener(NextTurn);
        
        base.Teardown();
    }

    public void SetIcons(List<CombatUnit> newUnitList)
    {
        Debug.Log("UITurnOrderWidget: SetIcons()");
        // iconList = newIconList;
        unitList = newUnitList;
        StartCoroutine(Refresh());
    }

    public void NextTurn()
    {
        indexStart++;

        if(indexStart >= unitList.Count) indexStart = 0;

        StartCoroutine(Refresh());
    }

    IEnumerator Refresh()
    {
        Debug.Log("UITurnOrderWidget: Starting Refresh Corouinte");
        Clear();
        yield return null;
        RenderIcons();
        Debug.Log("UITurnOrderWidget: Ending Refresh Corouinte");
    }

    private void Clear()
    {
        foreach (Transform child in iconParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void RenderIcons()
    {
        int x = 0;
        
        for (int i = indexStart; i < unitList.Count; i++)
        {
            GameObject obj = Instantiate(iconPrefab, iconParent.transform);
            UITurnOrderIcon icon = obj.GetComponent<UITurnOrderIcon>();
            
            icon.SetCombatUnit(unitList[i]);
            if(i == indexStart) icon.SetIsTurn(true);

            obj.transform.SetSiblingIndex(x);
            x++;
        }
        for (int i = 0; i < indexStart; i++)
        {
            GameObject obj = Instantiate(iconPrefab, iconParent.transform);
            UITurnOrderIcon icon = obj.GetComponent<UITurnOrderIcon>();
            
            icon.SetCombatUnit(unitList[i]);
            icon.SetHasGone(true);


            obj.transform.SetSiblingIndex(x);
            x++;
        }
    }
}
