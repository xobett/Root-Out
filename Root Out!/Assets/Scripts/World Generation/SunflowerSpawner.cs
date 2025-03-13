using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SunflowerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject sunflowerPrefab;
    [SerializeField] private GameObject fogSpawnerPrefab;

    [SerializeField] private int sunflowersToSpawn;

    [SerializeField] private List<int> lastPositions = new List<int>();

    private int spawnPos;

    void Start()
    {
        SpawnSunflower();
    }

    private void SpawnSunflower()
    {
        if (spawnPositions != null) 
        {
            StartCoroutine(AssignSpawnPosition());
        }
    }

    private void SpawnFogSpawners()
    {
        foreach (Transform spawnPosition in spawnPositions)
        {
            GameObject fogSpawnerClone = Instantiate(fogSpawnerPrefab, spawnPosition.position, spawnPosition.rotation);
            fogSpawnerClone.transform.parent = gameObject.transform.parent;
        }
    }

    private bool VerifyUsedPositions(int positionToVerify)
    {
        bool posUsed = false;

        if (lastPositions.Contains(positionToVerify))
        {
            posUsed = true;
        }
        return posUsed;
    }

    private IEnumerator AssignSpawnPosition()
    {
        for (int i = 0; i < sunflowersToSpawn; i++)
        {
            spawnPos = RandomPos();

            while (VerifyUsedPositions(spawnPos))
            {
                spawnPos = RandomPos();
                yield return null;
            }

            GameObject clone = Instantiate(sunflowerPrefab, spawnPositions[spawnPos].position, spawnPositions[spawnPos].rotation);
            clone.transform.parent = gameObject.transform.parent;

            lastPositions.Add(spawnPos);
        }

        SpawnFogSpawners();

        yield return null;
    }

    private int RandomPos()
    {
        int spawnPos = Random.Range(0, spawnPositions.Length);

        return spawnPos;
    }

}

