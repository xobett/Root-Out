using System.Collections;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [Header("ENEMY SPAWN SETTINGS")]
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject[] enemiesPrefabs;

    [SerializeField] private GameObject enemySpawnVfx;

    private GameObject lastEnemySpawned;

    [SerializeField] private AudioClip spawnClip;

    [ContextMenu("Spawn Enemy")]
    public void StartSpawner()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        GameObject enemyToSpawn = GetRandomEnemy();
        Transform spawnPosition = GetRandomSpawnPosition();
        Vector3 position = spawnPosition.position;

        while (enemyToSpawn == lastEnemySpawned)
        {
            enemyToSpawn = GetRandomEnemy();
            yield return null;
        }

        position.y = 1.5f;

        GameObject spawnVfx = Instantiate(enemySpawnVfx, position, Quaternion.identity);

        AudioSource spawnAudioSource = spawnPosition.gameObject.GetComponent<AudioSource>();
        spawnAudioSource.clip = spawnClip;
        spawnAudioSource.Play();


        yield return new WaitForSeconds(3.5f);

        GameObject enemyClone = Instantiate(enemyToSpawn, position, Quaternion.identity);
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
