using System;
using UnityEngine;

public abstract class GameEvent : ScriptableObject{}

public abstract class GameEvent<T> : GameEvent
{
    public event Action<T> Callback;

    public void Notify(T value)
    {
        Callback?.Invoke(value);
    }
}
