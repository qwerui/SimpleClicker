using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// BaseEventData 매개변수를 콜백으로 받는 이벤트
/// </summary>
[CreateAssetMenu(fileName = "TriggerEvent", menuName = "Events/TriggerEvent")]
public class TriggerEvent : GameEvent<BaseEventData>
{
    
}
