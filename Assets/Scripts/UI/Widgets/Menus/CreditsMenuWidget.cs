using UnityEngine;

public class CreditsMenuWidget : UIMenu
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

    public void OnClickBack()
    {
        StartCoroutine(Delay(BackButton, 0.1f));
    }

    private void BackButton()
    {
        requestLoadUIWidgetChannel.Raise(mainMenuWidgetData);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }
}
