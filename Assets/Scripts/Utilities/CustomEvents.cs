using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public class CustomEvents : MonoBehaviour
{
    
}

[System.Serializable]
public class IntEvent : UnityEvent<int> {}

[System.Serializable]
public class BooleanEvent : UnityEvent<bool> {}

[System.Serializable]
public class FloatEvent : UnityEvent<float> {}

[System.Serializable]
public class StringEvent : UnityEvent<string> {}

[System.Serializable]
public class Vector2Event : UnityEvent<Vector2> {}

[System.Serializable]
public class Vector3Event : UnityEvent<Vector3> {}

[System.Serializable]
public class SFXEvent : UnityEvent<string, Vector3> {}

[System.Serializable]
public class SongEvent : UnityEvent<SongData> {}

[System.Serializable]
public class PlayerSpawnInfoEvent : UnityEvent<PlayerSpawnInfo> {}

[System.Serializable]
public class GameObjectEvent : UnityEvent<GameObject> {}

[System.Serializable]
public class UIWidgetDataEvent : UnityEvent<UIWidgetData> {}

[System.Serializable]
public class SceneEvent : UnityEvent<SceneData> {}

[System.Serializable]
public class SceneContextEvent : UnityEvent<SceneContext> {}

[System.Serializable]
public class SpriteListEvent : UnityEvent<List<Sprite>> {}

[System.Serializable]
public class CombatUnitEvent : UnityEvent<CombatUnit> {}

[System.Serializable]
public class CombatUnitListEvent : UnityEvent<List<CombatUnit>> {}