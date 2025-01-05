using System;
using UnityEngine;

public class PlayerState
{
    [SerializeField] private float clickDamage = 1;
    [SerializeField] private long gold = 0;
    [SerializeField] long totalGold = 0;
    [SerializeField] private int clickCount = 0;
    [SerializeField] private int enemyKillCount = 0;

    public event Action OnGoldChange;

    public float ClickDamage 
    { 
        set { clickDamage = value; }
        get { return clickDamage; } 
    }

    public long Gold
    {
        set 
        { 
            gold = value;
            totalGold += value;
            OnGoldChange?.Invoke();
        }
        get { return gold; }
    }

    public long TotalGold { set => totalGold = value; get => totalGold; }

    public int ClickCount
    {
        set { clickCount = value; }
        get { return clickCount; }
    }

    public int EnemyKillCount
    {
        set { enemyKillCount = value; }
        get { return enemyKillCount; }
    }
}
