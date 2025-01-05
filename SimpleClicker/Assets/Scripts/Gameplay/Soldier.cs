using UnityEngine;

public class Soldier : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private IntegerEvent enemyDamageEvent;
    [SerializeField] private SoldierModel soldierModel;

    private Animator animator;

    private float attackTimeSum;
    private float randomAttackDelay;

    private void Awake()
    {
        TryGetComponent(out animator);
        randomAttackDelay = Random.Range(-0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.step != GameManager.GameStep.Playing)
        {
            return;
        }

        if(attackTimeSum >= soldierModel.AttackDelay + randomAttackDelay)
        {
            Attack();
            attackTimeSum = 0;
            randomAttackDelay = Random.Range(-0.2f, 0.2f);
        }
        attackTimeSum += Time.deltaTime;
    }

    void Attack()
    {
        enemyDamageEvent.Notify(soldierModel.Damage);
        animator.SetTrigger("doAttack");
    }
}
