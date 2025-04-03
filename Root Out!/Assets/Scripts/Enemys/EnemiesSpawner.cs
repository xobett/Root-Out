using System.Collections;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [Header("ENEMY SPAWN SETTINGS")]
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject[] enemiesPrefabs;

    [SerializeField] private GameObject enemySpawnVfx;

    private GameObject lastEnemySpawned;

    [ContextMenu("Spawn Enemy")]
    public void StartSpawner()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        GameObject enemyToSpawn = GetRandomEnemy();
        Vector3 spawnPosition = GetRandomSpawnPosition().position;

        while (enemyToSpawn == lastEnemySpawned)
        {
            enemyToSpawn = GetRandomEnemy();
            yield return null;
        }

        spawnPosition.y = 1.5f;

        GameObject spawnVfx = Instantiate(enemySpawnVfx, spawnPosition, Quaternion.identity);

        yield return new WaitForSeconds(3.5f);

        GameObject enemyClone = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        lastEnemySpawned = enemyToSpawn;

        yield return new WaitForSeconds(GameManager.instance.enemySpawnTime);

        if (GameManager.instance.eventTimerIsActive)
        {
            StartCoroutine(SpawnEnemies());
        }
        else
        {
            StopCoroutine(SpawnEnemies());
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
