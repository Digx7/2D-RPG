using UnityEngine;

public class QuitMenuWidget : UIMenu
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

    public void OnClickNo()
    {
        StartCoroutine(Delay(NoButton, 0.1f));
    }

    private void NoButton()
    {
        requestLoadUIWidgetChannel.Raise(mainMenuWidgetData);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void OnClickYes()
    {
        StartCoroutine(Delay(YesButton, 0.1f));
    }

    private void YesButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
