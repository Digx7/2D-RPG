using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UITurnOrderWidget : UIWidget
{
    public GameObject iconPrefab;
    public GameObject iconParent;
    public SpriteListChannel onSetupChannel;
    public Channel onNextTurnChannel;

    private List<Sprite> iconList;
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

    public void SetIcons(List<Sprite> newIconList)
    {
        iconList = newIconList;
        StartCoroutine(Refresh());
    }

    public void NextTurn()
    {
        indexStart++;

        if(indexStart >= iconList.Count) indexStart = 0;

        StartCoroutine(Refresh());
    }

    IEnumerator Refresh()
    {
        Clear();
        yield return null;
        RenderIcons();
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
        
        for (int i = indexStart; i < iconList.Count; i++)
        {
            GameObject obj = Instantiate(iconPrefab, iconParent.transform);
            UITurnOrderIcon icon = obj.GetComponent<UITurnOrderIcon>();
            icon.Render(iconList[i]);
            obj.transform.SetSiblingIndex(x);
            x++;
        }
        for (int i = 0; i < indexStart; i++)
        {
            GameObject obj = Instantiate(iconPrefab, iconParent.transform);
            UITurnOrderIcon icon = obj.GetComponent<UITurnOrderIcon>();
            icon.Render(iconList[i]);
            obj.transform.SetSiblingIndex(x);
            x++;
        }
    }
}
