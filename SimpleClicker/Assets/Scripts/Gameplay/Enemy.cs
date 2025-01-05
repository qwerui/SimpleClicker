using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private PlayerState playerState;
    [SerializeField] private EnemyModel enemyModel;
    
    [Header("Events")]
    [SerializeField] private TriggerEvent clickEvent;
    [SerializeField] private IntegerEvent enemyDamageEvent;
    [SerializeField] private VoidEvent enemyDiedEvent;

    private Action<int> hitPointUpdate;

    private Animator animator;
    private UnitChaseUI hitPointBar;

    private void OnEnable()
    {
        playerState = GameManager.Instance.playerState;

        clickEvent.Callback += DamageByClick;
        enemyDamageEvent.Callback += Damage;
        enemyDiedEvent.Callback += Die;

        hitPointBar = GetComponent<UnitChaseUI>();
        hitPointBar.Init();

        // 체력바 생성 및 업데이트 등록
        var filler = hitPointBar.Instance.transform.Find("Filler").GetComponent<Image>();
        hitPointUpdate = (hitPoint) => {
            filler.fillAmount = enemyModel.HitPointPercent; 
        };
        enemyModel.OnHitPointChanged.Callback += hitPointUpdate;
    }

    private void Start()
    {
        TryGetComponent(out animator);
    }

    private void DamageByClick(BaseEventData evt)
    {
        playerState.ClickCount++;
        enemyDamageEvent.Notify(Mathf.CeilToInt(playerState.ClickDamage));
        animator.SetTrigger("doTouch");
    }

    private void Damage(int amount)
    {
        playerState.Gold += amount;
        enemyModel.HitPoint -= amount;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        clickEvent.Callback -= DamageByClick;
        enemyDamageEvent.Callback -= Damage;
        enemyDiedEvent.Callback -= Die;

        enemyModel.OnHitPointChanged.Callback -= hitPointUpdate;
        Destroy(hitPointBar);
    }
}
