using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewActiveTimeLore", menuName = "ScriptableObjects/Lore/ActiveTime", order = 1)]
public class ActiveTimeLore : ScriptableObject
{
    public string title;

    [TextArea]
    public string info;

    public Sprite profilePic;
    public Sprite detailPic;
}

[System.Serializable]
public class ActiveTimeLoreEvent : UnityEvent<ActiveTimeLore> {}