using UnityEngine;

public class UIWidget : MonoBehaviour
{
    
    protected UIWidgetData ownUIWidgetData;
    [SerializeField] private UIWidgetDataChannel RequestUnloadWidgetDataChannel;

    public virtual void Setup(UIWidgetData newUIWidgetData)
    {
        ownUIWidgetData = newUIWidgetData;
    }

    public virtual void Teardown()
    {
        Destroy(this.gameObject);
    }

    protected void UnloadSelf()
    {
        RequestUnloadWidgetDataChannel.Raise(ownUIWidgetData);
    }
}
