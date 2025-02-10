using System.Collections;
using UnityEngine;

public class SunflowerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject sunflowerPrefab;

    private int sunflowersToSpawn = 2;

    private int spawnPos;
    private int lastSpawnPos;

    void Start()
    {
        SpawnSunflower();
        //SecondSpawn();
    }

    private void SecondSpawn()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            Instantiate(sunflowerPrefab, spawnPositions[i].position, spawnPositions[i].rotation);
        }
    }

    private void SpawnSunflower()
    {
        //spawn a sunflower in a spawnposition, while its still 2

        if (spawnPositions != null)
        {
            for (int i = 0; i <= sunflowersToSpawn; i++)
            {
                StartCoroutine(AssignCorrectPos());
                i++;
            } 
        }
    }

    private IEnumerator AssignCorrectPos()
    {
        spawnPos = RandomPos();

        while (spawnPos == lastSpawnPos)
        {
            spawnPos = RandomPos();
            yield return null;
        }

        Instantiate(sunflowerPrefab, spawnPositions[spawnPos].position, spawnPositions[spawnPos].rotation);
        Debug.Log(spawnPos);

        lastSpawnPos = spawnPos;

        yield return null;
    }


    private int RandomPos()
    {
        int spawnPos = Random.Range(0, spawnPositions.Length);

        return spawnPos;
    }
}

