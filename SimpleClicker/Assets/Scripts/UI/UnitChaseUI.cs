using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// 오브젝트를 추적하는 UI
/// </summary>
public class UnitChaseUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject targetUI;
    [SerializeField] private string targetCanvas; // 캔버스 이름

    [Header("Settings")]
    [SerializeField] private Vector2 offset;

    private Dictionary<Type, Component> componentsCache = new Dictionary<Type, Component>();
    private RectTransform foundCanvas;

    public GameObject Instance { get { return targetUI; } }

    public void Init()
    {
        foundCanvas = GameObject.Find(targetCanvas).GetComponent<RectTransform>();
        targetUI = Instantiate(targetUI, foundCanvas);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (foundCanvas != null) // 기준이 설정되면 위치 반영
        {
            Vector3 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);

            // pivot이 중앙이기 때문에 0.5를 뺌
            viewportPoint.x = foundCanvas.sizeDelta.x * (viewportPoint.x - 0.5f) + offset.x;
            viewportPoint.y = foundCanvas.sizeDelta.y * (viewportPoint.y - 0.5f) + offset.y;

            GetComponentFromUI<RectTransform>().anchoredPosition = viewportPoint;
        }
    }

    public T GetComponentFromUI<T>() where T : Component
    {
        if(componentsCache.ContainsKey(typeof(T)))
        {
            return (T)componentsCache[typeof(T)];
        }

        var component = targetUI.GetComponent<T>();

        if(component != null )
        {
            componentsCache[typeof(T)] = component;
        }

        return component;
    }

    private void OnDisable()
    {
        Destroy(targetUI);
    }
}
