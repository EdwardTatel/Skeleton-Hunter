using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemySpawns;
    [SerializeField] private AssetReference prefabsToSpawn;
    [SerializeField] private int enemyCount = 1;

    public int EnemyCount
    {
        get { return enemyCount; }
        set { enemyCount = value; }
    }
    private void Update()
    {
        if (enemyCount < 15 && AsyncLoader.startGame) 
        {
            int randomIndex = Random.Range(0, enemySpawns.Count);

            GameObject spawnPoint = enemySpawns[randomIndex];

            var loadOperation = prefabsToSpawn.InstantiateAsync(spawnPoint.transform.position, Quaternion.identity);

            loadOperation.Completed += (AsyncOperationHandle<GameObject> obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    obj.Result.transform.parent = spawnPoint.transform;
                }
                else
                {
                    Debug.LogError("Failed to instantiate prefab: " + obj.DebugName);
                }
            };
        }

    }

}
