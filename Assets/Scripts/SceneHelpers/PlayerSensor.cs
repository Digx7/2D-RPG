using UnityEngine;
using UnityEngine.Events;

public class PlayerSensor : MonoBehaviour
{
    public string PlayerTag;
    public UnityEvent onSensePlayer;
    public bool OnlyFireOnce = false;
    private bool hasBeenDetected = false;
    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == PlayerTag) SensedPlayer();
    }

    private void SensedPlayer()
    {
        if(OnlyFireOnce && hasBeenDetected) return;

        if(!hasBeenDetected) hasBeenDetected = true;
        
        onSensePlayer.Invoke();
    }
}
