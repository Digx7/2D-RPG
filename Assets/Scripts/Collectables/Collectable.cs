using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Collectable : MonoBehaviour
{
    public string PlayerTag = "Player";
    public int ID;
    public IntChannel onGetCollectableChannel;
    public UnityEvent onCollect;
    
    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag == PlayerTag)
        {
            Collect();
        }
    }

    private void Collect()
    {
        onGetCollectableChannel.Raise(ID);
        onCollect.Invoke();
        Destroy(gameObject);
    }
}
