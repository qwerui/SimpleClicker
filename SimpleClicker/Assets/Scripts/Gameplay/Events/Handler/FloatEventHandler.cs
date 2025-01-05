using UnityEngine;
using UnityEngine.Events;

public class FloatEventHandler : MonoBehaviour
{
    public FloatEvent eventSO;
    public UnityEvent<float> callback;

    private void OnEnable()
    {
        eventSO.Callback += callback.Invoke;
    }

    private void OnDisable()
    {
        eventSO.Callback -= callback.Invoke;
    }
}
