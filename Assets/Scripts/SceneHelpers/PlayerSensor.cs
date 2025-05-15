using UnityEngine;
using UnityEngine.Events;

public class PlayerSensor : MonoBehaviour
{
    public string PlayerTag;
    public UnityEvent onSensePlayer;
    public UnityEvent onSensePlayerLeave;
    public bool OnlyFireOnce = false;
    private bool hasBeenDetected = false;
    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == PlayerTag) SensedPlayerEnter();
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == PlayerTag) SensedPlayerExit();
    }

    private void SensedPlayerEnter()
    {
        if(OnlyFireOnce && hasBeenDetected) return;

        if(!hasBeenDetected) hasBeenDetected = true;
        
        onSensePlayer.Invoke();
    }

    private void SensedPlayerExit()
    {
        if(OnlyFireOnce && hasBeenDetected) return;

        if(!hasBeenDetected) hasBeenDetected = true;
        
        onSensePlayerLeave.Invoke();
    }
}
