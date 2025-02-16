using UnityEngine;

public class CombatMode : GameMode
{
    [SerializeField] private UIWidgetDataChannel requestLoadUIWidgetChannel;
    [SerializeField] private Channel requestLoadSaveDataChannel;
    [SerializeField] private Channel requestCombatStartChannel;
    [SerializeField] private UIWidgetData pauseMenuWidgetData;
    
    public override void Setup()
    {
        // add code here
        requestCombatStartChannel.Raise();
        
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