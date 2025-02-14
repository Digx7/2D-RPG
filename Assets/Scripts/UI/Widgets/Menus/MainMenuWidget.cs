using UnityEngine;

public class MainMenuWidget : UIMenu
{
    [SerializeField] SceneData gamePlayScene;
    [SerializeField] UIWidgetData optionsMenuWidgetData;
    [SerializeField] UIWidgetData creditsMenuWidgetData;
    [SerializeField] UIWidgetData quitMenuWidgetData;
    
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

    public void OnClickPlay()
    {
        StartCoroutine(Delay(PlayButton, 0.1f));
    }
    
    private void PlayButton()
    {
        requestChangeSceneChannel.Raise(gamePlayScene);
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

    public void OnClickCredits()
    {
        StartCoroutine(Delay(CreditsButton, 0.1f));
    }

    private void CreditsButton()
    {
        requestLoadUIWidgetChannel.Raise(creditsMenuWidgetData);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void OnClickQuit()
    {
        StartCoroutine(Delay(QuitButton, 0.1f));
    }

    private void QuitButton()
    {
        requestLoadUIWidgetChannel.Raise(quitMenuWidgetData);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }
}
