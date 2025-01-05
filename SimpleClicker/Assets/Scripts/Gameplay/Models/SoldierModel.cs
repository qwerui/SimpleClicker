using UnityEngine;

[CreateAssetMenu(fileName = "SoldierModel", menuName = "Gameplay/SoldierModel")]
public class SoldierModel : ScriptableObject
{
    public enum SoldierType
    {
        Sword,
        Bow,
        Magic,
        Shield
    }

    public SoldierType type;
    public GameObject prefab;

    public InventoryData damageUpgrade;
    public InventoryData delayUpgrade;
    public InventoryData spawnUpgrade;

    [SerializeField] private int baseDamage;
    [SerializeField] private float baseDelay;
    [SerializeField] private int damageCoefficent;
    [SerializeField] private float delayCoefficent;

    public int Damage { get => baseDamage + damageUpgrade.UpgradeCount * damageCoefficent; }
    public float AttackDelay { get => baseDelay - delayUpgrade.UpgradeCount * delayCoefficent; }
}

