using UnityEngine;
using UnityEngine.Events;

public class VoidEventHandler : MonoBehaviour
{
    public VoidEvent eventSO;
    public UnityEvent callback;

    private void OnEnable()
    {
        eventSO.Callback += callback.Invoke;
    }

    private void OnDisable()
    {
        eventSO.Callback -= callback.Invoke;
    }
}
