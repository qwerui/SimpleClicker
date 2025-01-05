using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TriggerEvnetHandler : MonoBehaviour
{
    public TriggerEvent eventSO;
    public UnityEvent<BaseEventData> callback;

    private void OnEnable()
    {
        eventSO.Callback += callback.Invoke;
    }

    private void OnDisable()
    {
        eventSO.Callback -= callback.Invoke;
    }
}
