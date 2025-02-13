using System.Collections;
using UnityEngine;

public class SunflowerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject sunflowerPrefab;

    [SerializeField] private int sunflowersToSpawn;

    private int spawnPos;
    private int lastSpawnPos;

    void Start()
    {
        SpawnSunflower();
    }

    private void SpawnSunflower()
    {
        //spawn a sunflower in a spawnposition, while its still 2

        if (spawnPositions != null)
        {
            for (int i = 0; i < sunflowersToSpawn; i++)
            {
                StartCoroutine(AssignCorrectPos());
                Debug.Log(i + 1);
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
        //Debug.Log(spawnPos);

        lastSpawnPos = spawnPos;

        yield return null;
    }


    private int RandomPos()
    {
        int spawnPos = Random.Range(0, spawnPositions.Length);

        return spawnPos;
    }
}

