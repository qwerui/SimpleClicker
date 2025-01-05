using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyModel", menuName = "Gameplay/EnemyModel")]
public class EnemyModel : ScriptableObject
{
    public enum EnemyStatus
    {
        Alive,
        Dead
    }
    [Header("Coefficient")]
    [SerializeField] private float Quadratic;
    [SerializeField] private float Linear;
    [SerializeField] private int Constant;

    [Header("Default - Not use in runtime")]
    [SerializeField] private int maxHitPoint; // 수식 기반 값
    [SerializeField] private int currentHitPoint;

    [HideInInspector]
    public EnemyStatus Status { private set; get; }
    public IntegerEvent OnHitPointChanged;
    public VoidEvent OnEnemyDied;

    public int HitPoint
    {
        set
        {
            currentHitPoint = Mathf.Clamp(value, 0, maxHitPoint);
            OnHitPointChanged.Notify(currentHitPoint);

            if(currentHitPoint == 0)
            {
                Status = EnemyStatus.Dead;
                OnEnemyDied.Notify();
            }
        }
        get { return currentHitPoint; }
    }

    public float HitPointPercent
    {
        get { return currentHitPoint / (float)maxHitPoint; }
    }

    public void Init(int enemyKillCount)
    {
        Status = EnemyStatus.Alive;
        maxHitPoint = (int)(enemyKillCount * enemyKillCount * Quadratic + Linear * enemyKillCount + Constant);
        currentHitPoint = maxHitPoint;
    }
}
