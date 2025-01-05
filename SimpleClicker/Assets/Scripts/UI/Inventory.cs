using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image toggleButtonSprite;
    [Header("Assets")]
    [SerializeField] private Sprite closedButtonSprite;
    [SerializeField] private Sprite openedButtonSprite;
    [Header("Contents")]
    [SerializeField] private Transform targetView;
    [SerializeField] private InventoryContent contentPrefab;
    [SerializeField] private List<InventoryData> inventoryDatas = new();

    private RectTransform rect;

    private bool isOpened;

    private void Awake()
    {
        isOpened = false;
        TryGetComponent(out rect);
    }

    private void Start()
    {
        inventoryDatas.Sort((a, b) => { return a.order - b.order; });

        foreach (var data in inventoryDatas)
        {
            var content = Instantiate(contentPrefab, targetView);
            content.data = data;
            content.Init();
        }
    }

    public void ToggleInventory()
    {
        if (isOpened)
        {
            // 닫기
            rect.DOAnchorPosX(0, 1);
            toggleButtonSprite.sprite = closedButtonSprite;
        }
        else
        {
            // 열기
            rect.DOAnchorPosX(-300, 1);
            toggleButtonSprite.sprite = openedButtonSprite;
        }

        isOpened = !isOpened;
    }
}
