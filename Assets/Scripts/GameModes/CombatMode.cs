using UnityEngine;

public class CombatMode : GameMode
{
    [SerializeField] private UIWidgetDataChannel requestLoadUIWidgetChannel;
    [SerializeField] private Channel requestLoadSaveDataChannel;
    [SerializeField] private UIWidgetData pauseMenuWidgetData;
    
    public override void Setup()
    {
        // add code here
        
        base.Setup();
    }

    public override void Teardown()
    {
        // add code here
        
        base.Teardown();
    }

    protected override void OnOptionsMenuQuit()
    {
        requestLoadUIWidgetChannel.Raise(pauseMenuWidgetData);
    }
}