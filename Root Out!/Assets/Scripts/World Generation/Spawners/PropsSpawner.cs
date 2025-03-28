using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class PropsSpawner : MonoBehaviour
{
    [Header("LOOT SETTINGS")]
    [SerializeField] private GameObject seedCoin;

    [SerializeField] private GameObject ammo;

    [SerializeField] private GameObject[] weapons;
    [SerializeField] private GameObject[] cropCards;
    [SerializeField] private GameObject[] perks;

    [Header("PROP SETTINGS")]
    [SerializeField] private GameObject[] simpleProps;
    [SerializeField] private GameObject[] coverProps;

    [Header("PROP SPAWN POSITION SETTINGS")]
    [SerializeField] private Transform[] simplePropsPos;
    [SerializeField] private Transform[] coverPropsPos;

    [Header("PROP POSITIONS GROUPS")]
    [SerializeField] private GameObject[] positionsGroups;

    private List<int> simplePropUsedPos = new List<int>();
    private List<int> coverPropUsedPos = new List<int>();

    void Start()
    {
        StartCoroutine(SpawnProps());
        StopCoroutine(SpawnProps());
    }

    private IEnumerator SpawnProps()
    {
        for (int i = 0; i < simplePropsPos.Length; i++)
        {
            GameObject lootItem = SpawnNewItem();

            int randomSimpleProp = GenerateRandomProp("Simple");
            int randomSimplePropPos = GenerateRandomPropPos("Simple");

            while (simplePosUsed(randomSimplePropPos))
            {
                randomSimplePropPos = GenerateRandomPropPos("Simple");
                yield return null;
            }

            if (lootItem != null)
            {
                GameObject clone = Instantiate(lootItem, simplePropsPos[randomSimplePropPos].position, lootItem.transform.rotation);

                clone.transform.parent = gameObject.transform.parent;

                simplePropUsedPos.Add(randomSimplePropPos); 
            }
        }

        for (int i = 0; i < coverPropsPos.Length; i++)
        {
            int randomCoverProp = GenerateRandomProp("Cover");
            int randomCoverPropPos = GenerateRandomPropPos("Cover");

            while (coverPosUsed(randomCoverPropPos))
            {
                randomCoverPropPos = GenerateRandomPropPos("Cover");
                yield return null;
            }

            GameObject clone = Instantiate(coverProps[randomCoverProp], coverPropsPos[randomCoverPropPos].position, coverProps[randomCoverProp].transform.rotation);
            clone.transform.parent = gameObject.transform.parent;

            coverPropUsedPos.Add(randomCoverPropPos);
        }

        ClearUnusedElements();

        yield return null;
    }

    private bool simplePosUsed(int posToVerify)
    {
        return simplePropUsedPos.Contains(posToVerify);
    }

    private bool coverPosUsed(int posToVerify)
    {
        return coverPropUsedPos.Contains(posToVerify);
    }

    private int GenerateRandomProp(string propType)
    {
        int randomPropNumber = 0;

        switch (propType)
        {
            case "Simple":
                {
                    randomPropNumber = Random.Range(0, simpleProps.Length);
                    break;
                }

            case "Cover":
                {
                    randomPropNumber = Random.Range(0, coverProps.Length);
                    break;
                }
        }

        return randomPropNumber;
    }

    private int GenerateRandomPropPos(string propType)
    {
        int randomPosNumber = 0;
        switch (propType)
        {
            case "Simple":
                {
                    randomPosNumber = Random.Range(0, simplePropsPos.Length);
                    break;
                }

            case "Cover":
                {
                    randomPosNumber = Random.Range(0, coverPropsPos.Length);
                    break;
                }
        }

        return randomPosNumber;
    }

    private void ClearUnusedElements()
    {
        for (int i = 0; i < positionsGroups.Length; i++)
        {
            Destroy(positionsGroups[i]);
        }

        Destroy(this.gameObject);
    }

    private GameObject SpawnNewItem()
    {
        GameObject itemToSpawn = null;

        //Dependiendo del resultado del random, se aumentara o bajara la probabilidad de que se spawnee el item que se elija.
        int randomItemSelection = Random.Range(0, 1);

        switch (randomItemSelection)
        {
            case 0:
                {
                    Debug.Log("Money item was chosen");
                    int seedProbability = Random.Range(0, 8);
                    
                    if (NumberIsPair(seedProbability))
                    {
                        itemToSpawn = seedCoin;
                    }
                    else
                    {
                        Debug.Log("Money did not make the cut");
                    }

                    break;
                }

            case 1:
                {
                    Debug.Log("Ammo item was chosen");
                    int ammoProbability = Random.Range(0, 3);

                    if (ammoProbability == 2)
                    {
                        itemToSpawn = ammo;
                    }
                    else
                    {
                        Debug.Log("Ammo did not make the cut");
                    }

                    break;
                }

            case 2:
                {
                    Debug.Log("Loot item was chosen");
                    int lootProbability = Random.Range(0, 4);

                    if (lootProbability == 3)
                    {
                        int lootSelection = Random.Range(0, 2);

                        switch (lootSelection)
                        {
                            case 0:
                                {
                                    itemToSpawn = cropCards[Random.Range(0, cropCards.Length)];
                                    break;
                                }

                            case 1:
                                {
                                    itemToSpawn = weapons[Random.Range(0, weapons.Length)];
                                    break;
                                }

                            case 2:
                                {
                                    itemToSpawn = perks[Random.Range(0, perks.Length)];
                                    break;
                                }
                        }

                    }
                    else
                    {
                        Debug.Log("Loot did not make the cut");
                    }

                    break;
                }
        }

        return itemToSpawn;
    }

    private bool NumberIsPair(int number)
    {
        if (number % 2 == 0) return true;
        else return false;
    }

}
