using UnityEngine;
using UnityEngine.Events;

public class IntegerEventHandler : MonoBehaviour
{
    public IntegerEvent eventSO;
    public UnityEvent<int> callback;

    private void OnEnable()
    {
        eventSO.Callback += callback.Invoke;
    }

    private void OnDisable()
    {
        eventSO.Callback -= callback.Invoke;
    }
}
