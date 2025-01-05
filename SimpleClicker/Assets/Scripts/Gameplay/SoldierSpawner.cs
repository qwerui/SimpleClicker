using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSpawner : MonoBehaviour
{
    public List<SoldierModel> models = new List<SoldierModel>();
    Dictionary<SoldierModel.SoldierType, GameObject> soldierPrefabs = new Dictionary<SoldierModel.SoldierType, GameObject>();
    Dictionary<SoldierModel.SoldierType, Action> spawnAction = new Dictionary<SoldierModel.SoldierType, Action>();

    private void OnEnable()
    {
        foreach (var model in models)
        {
            soldierPrefabs[model.type] = model.prefab;
            spawnAction[model.type] = () => Spawn(soldierPrefabs[model.type]);
            model.spawnUpgrade.OnUpgrade += spawnAction[model.type];
        }
    }

    private void Start()
    {
        foreach (var model in models)
        {
            for (int i = 0; i < model.spawnUpgrade.UpgradeCount; i++)
            {
                Spawn(model.prefab);
            }
        }
    }

    void Spawn(GameObject obj)
    {
        Instantiate(obj, new Vector3(UnityEngine.Random.Range(-3f, -1f), UnityEngine.Random.Range(-1f, 0f)), Quaternion.identity, transform);
    }

    void OnDisable()
    {
        foreach(var model in models)
        {
            model.spawnUpgrade.OnUpgrade -= spawnAction[model.type];
        }
    }
}
