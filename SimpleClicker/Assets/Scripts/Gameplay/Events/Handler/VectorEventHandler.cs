using UnityEngine;
using UnityEngine.Events;

public class VectorEventHandler : MonoBehaviour
{
    public VectorEvent eventSO;
    public UnityEvent<Vector3> callback;

    private void OnEnable()
    {
        eventSO.Callback += callback.Invoke;
    }

    private void OnDisable()
    {
        eventSO.Callback -= callback.Invoke;
    }
}
