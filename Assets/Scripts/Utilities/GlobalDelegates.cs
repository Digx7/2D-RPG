using UnityEngine;
using UnityEngine.Events;
using System;

public class GlobalDelegates
{
    
}

public delegate void VoidDelegate();
public delegate void IntDelegate(int value);
public delegate void IntTypeDelegate(int value, Type type);
public delegate void FloatDelegate(float value);
public delegate void BoolDelegate(bool value);
public delegate void Vector2Delegate(Vector2 value);
public delegate void Vector3Delegate(Vector3 value);
public delegate void ObjectDelegate(object obj);