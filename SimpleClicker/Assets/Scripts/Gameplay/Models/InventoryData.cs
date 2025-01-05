using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Gameplay/InventoryData")]
public class InventoryData : ScriptableObject
{
    [Header("Info")]
    public int id;
    public Sprite Icon;
    public string Description;
    public int order;

    [Header("Unlock")]
    public List<InventoryData> preRequire = new List<InventoryData>();
    public bool Locked
    {
        get
        {
            foreach (var require in preRequire)
            {
                if(require.upgradeCount == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }

    [Header("Gold Coefficient")]
    public float Quadratic;
    public float Linear;
    public int Constant;

    public int MaxUpgrade;
    [NonSerialized] private int upgradeCount;
    public int UpgradeCount
    {
        set 
        { 
            if(upgradeCount < MaxUpgrade)
            {
                upgradeCount = value;
                OnUpgrade?.Invoke();
            }
        }
        get => upgradeCount;
    }
    public string GetUpgradeCountString()
    {
        return upgradeCount == MaxUpgrade ? "Max" : upgradeCount.ToString(); 
    }

    public event Action OnUpgrade;
    
    public int RequireGold
    {
        get
        {
            return (int)(upgradeCount * upgradeCount * Quadratic + Linear * upgradeCount + Constant);
        }
    }

    public string GetRequireGoldString()
    {
        return upgradeCount == MaxUpgrade ? "-" : Formatter.ShortenInteger(RequireGold);
    }
}
