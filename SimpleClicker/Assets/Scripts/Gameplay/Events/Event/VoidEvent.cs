using System;
using UnityEngine;

/// <summary>
/// 매개변수가 없는 콜백을 받는 이벤트
/// </summary>
[CreateAssetMenu(fileName = "VoidEvent", menuName = "Events/VoidEvent")]
public class VoidEvent : GameEvent
{
    public event Action Callback;

    public void Notify()
    {
        Callback?.Invoke();
    }
}
