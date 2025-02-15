using UnityEngine;

public class PauseMenuWidget : UIMenu
{
    [SerializeField] SceneData mainMenuScene;
    [SerializeField] UIWidgetData optionsMenuWidgetData;
    
    [SerializeField] SceneChannel requestChangeSceneChannel;
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
        StartCoroutine(Delay(ResumeButton, 0.1f));
    }

    private void ResumeButton()
    {
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void OnClickOptions()
    {
        StartCoroutine(Delay(OptionsButton, 0.1f));
    }

    private void OptionsButton()
    {
        requestLoadUIWidgetChannel.Raise(optionsMenuWidgetData);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void OnClickQuit()
    {
        StartCoroutine(Delay(QuitButton, 0.1f));
    }

    private void QuitButton()
    {
        requestChangeSceneChannel.Raise(mainMenuScene);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }
}
