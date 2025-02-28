using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SunflowerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject sunflowerPrefab;

    [SerializeField] private int sunflowersToSpawn;

    [SerializeField] private List<int> lastPositions = new List<int>();

    //[SerializeField] private int[] usedPositions = new int[3];

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
            Debug.Log($"Entrada numero {i + 1}");
            spawnPos = RandomPos();
            Debug.Log($"Numero inicial: {spawnPos}");

            while (VerifyUsedPositions(spawnPos))
            {
                spawnPos = RandomPos();
                yield return null;
            }

            GameObject clone = Instantiate(sunflowerPrefab, spawnPositions[spawnPos].position, spawnPositions[spawnPos].rotation);
            clone.transform.parent = gameObject.transform.parent;
            Debug.Log($"Position {spawnPos} was used and added");

            //usedPositions[i] = spawnPos;
            lastPositions.Add(spawnPos);
        }

        yield return null;
    }


    private int RandomPos()
    {
        int spawnPos = Random.Range(0, spawnPositions.Length);

        return spawnPos;
    }

    //private bool VerifyLastPositions(int positionToVerify)
    //{
    //    bool zeroUsed = false;
    //    bool positionUsed = false;

    //    foreach (int usedPosition in usedPositions)
    //    {
    //        Debug.Log($"Number to verify {positionToVerify} and used position {usedPosition}");
    //        if (positionToVerify == usedPosition)
    //        {

    //            positionUsed = true;
    //            Debug.Log($"Position {usedPosition} was already used.");

    //            if (positionToVerify == 0 && usedPosition == 0 && !zeroUsed)
    //            {
    //                Debug.Log("Aqui entra el 0 usado por primera vez");
    //                zeroUsed = true;
    //                positionUsed = false;
    //            }

    //        }

    //        Debug.Log($"Regresa {positionUsed}");
    //    }
    //    return positionUsed;
    //}
}

