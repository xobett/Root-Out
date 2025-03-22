using System.Collections;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [Header("ENEMY SPAWN SETTINGS")]
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private float timeBetweenSpawn;
    [SerializeField, Tooltip("Enemies prefabs to instantiate.")] private GameObject[] enemiesPrefabs;

    [ContextMenu("Spawn Enemy")]
    public void StartSpawner()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(timeBetweenSpawn);

        Instantiate(GetRandomEnemy(), GetRandomSpawnPosition().position, Quaternion.identity);

        if (GameManager.instance.eventTimerisActive)
        {
            StartCoroutine(SpawnEnemies());
        }
        else
        {
            yield return null;
        }
    }

    private Transform GetRandomSpawnPosition()
    {
        int randomSpawnPosition = Random.Range(0, spawnPositions.Length);

        return spawnPositions[randomSpawnPosition];
    }

    private GameObject GetRandomEnemy()
    {
        int randomPrefabIndex = Random.Range(0, enemiesPrefabs.Length);

        return enemiesPrefabs[randomPrefabIndex];
    }
}
