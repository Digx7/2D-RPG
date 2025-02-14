using UnityEngine;
using System;
using System.Collections;

public class SplashScreenMenuWidget : UIMenu
{
    [SerializeField] UIWidgetData mainMenuWidgetData;
    
    [SerializeField] UIWidgetDataChannel requestLoadUIWidgetChannel;
    [SerializeField] UIWidgetDataChannel requestUnLoadUIWidgetChannel;

    public override void Setup(UIWidgetData newUIWidgetData)
    {
        base.Setup(newUIWidgetData);
    }

    public override void Teardown()
    {
        base.Teardown();
    }

    public void OnClickStart()
    {
        StartCoroutine(Delay(StartButton, 0.1f));
    }

    private void StartButton()
    {
        requestLoadUIWidgetChannel.Raise(mainMenuWidgetData);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }
}
