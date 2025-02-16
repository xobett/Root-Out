using System.Collections;
using UnityEngine;

public class SunflowerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject sunflowerPrefab;

    [SerializeField] private int[] usedPositions = new int[3];

    [SerializeField] private int sunflowersToSpawn;

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
            for (int i = 0; i < sunflowersToSpawn; i++)
            {
                //StartCoroutine(AssignCorrectPos());

                spawnPos = RandomPos();

                Debug.Log(spawnPos);

                usedPositions[i] = spawnPos;

            } 
        }
    }

    private IEnumerator AssignCorrectPos()
    {
        Debug.Log("Entro");
        spawnPos = RandomPos();

        VerifyLastPositions(spawnPos);

        //Instantiate(sunflowerPrefab, spawnPositions[spawnPos].position, spawnPositions[spawnPos].rotation);
        Debug.Log(spawnPos);
        yield return null;

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

    private int RandomPos()
    {
        int spawnPos = Random.Range(0, spawnPositions.Length);

        return spawnPos;
    }
}

