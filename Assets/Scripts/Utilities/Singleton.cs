using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    public virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Another Singleton " + Instance + " was found so deleting this one");
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);

        Instance = this as T;
    }
}
