using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class InteractZone : MonoBehaviour
{

    public string playerTag = "Player";
    public Channel onPlayerTryInteractChannel;
    public Channel onStartInteractionChannel;
    public Channel onStopInteractionChannel;
    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerLeave;
    public UnityEvent onPlayerInteract;
    public UnityEvent onInteractionStart;
    public UnityEvent onInteractionStop;
    

    private bool isPlayerInZone = false;

    private void OnEnable()
    {
        onPlayerTryInteractChannel.channelEvent.AddListener(TryInteract);
        onStartInteractionChannel.channelEvent.AddListener(OnStartInteraction);
        onStopInteractionChannel.channelEvent.AddListener(OnStopInteraction);
    }

    private void OnDisable()
    {
        onPlayerTryInteractChannel.channelEvent.RemoveListener(TryInteract);
        onStartInteractionChannel.channelEvent.RemoveListener(OnStartInteraction);
        onStopInteractionChannel.channelEvent.RemoveListener(OnStopInteraction);
    }

    public void TryInteract()
    {
        if(isPlayerInZone) 
        {
            onPlayerInteract.Invoke();
            Debug.Log("InteractZone: Interact");
        }
    }

    public void OnStartInteraction()
    {
        if(isPlayerInZone) onInteractionStart.Invoke();
    }

    public void OnStopInteraction()
    {
        if(isPlayerInZone) onInteractionStop.Invoke();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == playerTag) 
        {
            Debug.Log("InteractZone: Player Entered Zone");
            onPlayerEnter.Invoke();
            isPlayerInZone = true;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == playerTag) 
        {
            Debug.Log("InteractZone: Player Left Zone");
            onPlayerLeave.Invoke();
            isPlayerInZone = false;
        }
    }
}