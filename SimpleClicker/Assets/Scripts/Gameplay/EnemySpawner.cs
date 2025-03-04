using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyModel model;
    [SerializeField] private PlayerState playerState;
    [SerializeField] private string enemyLabel;

    private AsyncOperationHandle enemyHandle;
    private IList<GameObject> enemyPrefabs;

    private void OnEnable()
    {
        playerState = GameManager.Instance.playerState;
        model.OnEnemyDied.Callback += Spawn;
    }

    private void Start()
    {
        var handle = Addressables.LoadAssetsAsync<GameObject>(enemyLabel);
        handle.Completed += (op) =>
        {
            enemyPrefabs = op.Result;
            Spawn();
        };
        enemyHandle = handle;
    }

    private void Spawn()
    {
        if(enemyPrefabs.Count == 0)
        {
            return;
        }

        Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], transform);
        model.Init(playerState.EnemyKillCount);
    }

    private void OnDisable()
    {
        enemyHandle.Release();
        model.OnEnemyDied.Callback -= Spawn;
    }
}
