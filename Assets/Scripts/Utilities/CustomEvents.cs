using UnityEngine;
using UnityEngine.Events;

public class CustomEvents : MonoBehaviour
{
    
}

public class IntEvent : UnityEvent<int> {}

public class BooleanEvent : UnityEvent<bool> {}

public class FloatEvent : UnityEvent<float> {}
public class StringEvent : UnityEvent<string> {}
public class Vector2Event : UnityEvent<Vector2> {}
public class Vector3Event : UnityEvent<Vector3> {}
public class SFXEvent : UnityEvent<string, Vector3> {}

public class SongEvent : UnityEvent<SongData> {}
public class PlayerSpawnInfoEvent : UnityEvent<PlayerSpawnInfo> {}
public class GameObjectEvent : UnityEvent<GameObject> {}
public class UIWidgetDataEvent : UnityEvent<UIWidgetData> {}
