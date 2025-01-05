using UnityEngine;

[ExecuteInEditMode]
public class SafeAreaEdit : MonoBehaviour
{
    private RectTransform rect;

    private void Awake() 
    {
        TryGetComponent(out rect);
    }
    // Update is called once per frame
    void Update()
    {
        Rect safeArea = Screen.safeArea;

        Vector2 minAnchor = safeArea.position;
        Vector2 maxAnchor = minAnchor + safeArea.size;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        rect.anchorMin = minAnchor;
        rect.anchorMax = maxAnchor;
    }
}
