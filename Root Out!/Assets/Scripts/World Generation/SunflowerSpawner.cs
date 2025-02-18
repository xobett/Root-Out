using System.Collections;
using UnityEngine;

public class SunflowerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject sunflowerPrefab;

    [SerializeField] private int sunflowersToSpawn;

    [SerializeField] private int[] usedPositions = new int[3];


    private int spawnPos;

    void Start()
    {
        SpawnSunflower();
    }

    private void SpawnSunflower()
    {
        //spawn a sunflower in a spawnposition, while its still 2

        if (spawnPositions != null)
        {
            StartCoroutine(AssignSpawnPosition());
        }
    }
    private bool VerifyLastPositions(int positionToVerify)
    {
        bool notUsed = false;

        foreach (int usedPosition in usedPositions)
        {
            if (positionToVerify == usedPosition)
            {
                notUsed = true;
                Debug.Log("Position was used.");
            }
        }

        return notUsed;
    }

    private IEnumerator AssignSpawnPosition()
    {
        for (int i = 0; i < sunflowersToSpawn; i++)
        {
            Debug.Log("Entro");
            spawnPos = RandomPos();

            while (VerifyLastPositions(spawnPos))
            {
                spawnPos = RandomPos();
                yield return null;
            }

            Instantiate(sunflowerPrefab, spawnPositions[spawnPos].position, spawnPositions[spawnPos].rotation);
            Debug.Log($"Position {spawnPos} was used and added");

            usedPositions[i] = spawnPos;
        }

        yield return null;
    }


    private int RandomPos()
    {
        int spawnPos = Random.Range(0, spawnPositions.Length);

        return spawnPos;
    }
}

