using UnityEngine;
using UnityEngine.Events;

public class PlayerSensor : MonoBehaviour
{
    public string PlayerTag;
    public UnityEvent onSensePlayer;
    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == PlayerTag) onSensePlayer.Invoke();
    }
}
