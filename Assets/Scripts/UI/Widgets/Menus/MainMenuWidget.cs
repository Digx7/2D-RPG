using UnityEngine;

public class MainMenuWidget : UIMenu
{
    [SerializeField] string gamePlayScene;
    [SerializeField] UIWidgetData optionsMenuWidgetData;
    [SerializeField] UIWidgetData creditsMenuWidgetData;
    [SerializeField] UIWidgetData quitMenuWidgetData;
    
    [SerializeField] StringChannel requestChangeSceneChannel;
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

    public void OnClickPlay()
    {
        requestChangeSceneChannel.Raise(gamePlayScene);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void OnClickOptions()
    {
        requestLoadUIWidgetChannel.Raise(optionsMenuWidgetData);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void OnClickCredits()
    {
        requestLoadUIWidgetChannel.Raise(creditsMenuWidgetData);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void OnClickQuit()
    {
        requestLoadUIWidgetChannel.Raise(quitMenuWidgetData);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }
}
