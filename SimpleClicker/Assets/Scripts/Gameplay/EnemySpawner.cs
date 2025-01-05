using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyModel model;
    [SerializeField] private PlayerState playerState;
    [SerializeField] private GameObject[] enemyPrefabs;

    private void OnEnable()
    {
        playerState = GameManager.Instance.playerState;
        model.OnEnemyDied.Callback += Spawn;
    }

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        if(enemyPrefabs.Length == 0)
        {
            return;
        }

        Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], transform);
        model.Init(playerState.EnemyKillCount);
    }

    private void OnDisable()
    {
        model.OnEnemyDied.Callback -= Spawn;
    }
}
