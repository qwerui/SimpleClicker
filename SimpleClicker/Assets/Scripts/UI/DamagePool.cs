using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class DamagePool : MonoBehaviour
{
    [Header("Position References")]
    public Transform pivot;
    public RectTransform canvas;

    [Header("References")]
    public Text damagePrefab;
    public IntegerEvent enemyDamageEvent;

    private RectTransform rectTransform;
    private IObjectPool<Text> pool;
    private Dictionary<GameObject, RectTransform> textRectCache; // Tween을 위한 RectTransform 캐싱

    private void Awake()
    {
        pool = new ObjectPool<Text>(Create, Get, Release, Destroy, false, maxSize: 20);
        textRectCache = new Dictionary<GameObject, RectTransform>();
    }

    private void OnEnable()
    {
        enemyDamageEvent.Callback += ShowDamage;
    }

    private void Start()
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(pivot.position);

        // pivot이 중앙이기 때문에 0.5를 뺌
        viewportPoint.x = canvas.sizeDelta.x * (viewportPoint.x - 0.5f);
        viewportPoint.y = canvas.sizeDelta.y * (viewportPoint.y - 0.5f);

        rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = viewportPoint;
    }

    private Text Create()
    {
        return Instantiate(damagePrefab, canvas);
    }

    private void Get(Text text)
    {
        text.gameObject.SetActive(true);

        if(!textRectCache.ContainsKey(text.gameObject))
        {
            textRectCache.Add(text.gameObject, text.GetComponent<RectTransform>());
        }

        RectTransform textRect = textRectCache[text.gameObject];

        textRect.anchoredPosition = rectTransform.anchoredPosition;

        float xEnd = Random.Range(rectTransform.anchoredPosition.x - 100, rectTransform.anchoredPosition.x + 100);
        float yEnd = Random.Range(rectTransform.anchoredPosition.y - 100, rectTransform.anchoredPosition.y - 50);

        
        Vector2 endPosition = new Vector2(xEnd, yEnd);

        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(textRect.DOAnchorPosY(rectTransform.anchoredPosition.y + 50, 0.2f))
            .Append(textRect.DOAnchorPosY(yEnd, 0.8f).SetEase(Ease.OutBounce))
            .Insert(0, textRect.DOAnchorPosX(xEnd, 1f))
            .OnComplete(() => { pool.Release(text); });

        sequence.Play();
    }

    private void Release(Text text)
    {
        text.gameObject.SetActive(false);
    }

    private void Destroy(Text text)
    {
        // 참조 해제
        textRectCache.Remove(text.gameObject);
        Destroy(text.gameObject);
    }

    private void ShowDamage(int damage)
    {
        Text get = pool.Get();

        get.text = Formatter.ShortenInteger(damage);
    }

    private void OnDisable()
    {
        enemyDamageEvent.Callback -= ShowDamage;
    }
}
