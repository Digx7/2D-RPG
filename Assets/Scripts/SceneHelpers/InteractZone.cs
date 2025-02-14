using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class InteractZone : MonoBehaviour
{

    public string playerTag = "Player";
    public Channel onPlayerTryInteractChannel;
    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerLeave;
    public UnityEvent onPlayerInteract;

    private bool isPlayerInZone = false;

    private void OnEnable()
    {
        onPlayerTryInteractChannel.channelEvent.AddListener(TryInteract);
    }

    private void OnDisable()
    {
        onPlayerTryInteractChannel.channelEvent.RemoveListener(TryInteract);
    }

    public void TryInteract()
    {
        if(isPlayerInZone) 
        {
            onPlayerInteract.Invoke();
            Debug.Log("InteractZone: Interact");
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.tag == playerTag) 
        {
            Debug.Log("InteractZone: Player Entered Zone");
            onPlayerEnter.Invoke();
            isPlayerInZone = true;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if(col.tag == playerTag) 
        {
            Debug.Log("InteractZone: Player Left Zone");
            onPlayerLeave.Invoke();
            isPlayerInZone = false;
        }
    }
}