using UnityEngine;

public class PauseMenuWidget : UIMenu
{
    [SerializeField] string mainMenuScene;
    [SerializeField] UIWidgetData optionsMenuWidgetData;
    
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

    public void OnClickResume()
    {
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void OnClickOptions()
    {
        requestLoadUIWidgetChannel.Raise(optionsMenuWidgetData);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void OnClickQuit()
    {
        requestChangeSceneChannel.Raise(mainMenuScene);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }
}
