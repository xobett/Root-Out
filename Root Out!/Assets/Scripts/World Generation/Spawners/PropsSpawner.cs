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

    [Header("LOOT SPAWN POSITION SETTINGS")]
    [SerializeField] private Transform[] lootPos;

    [Header("ITEM SHOP SETTINGS")]
    [SerializeField] private GameObject nestMarket;

    [SerializeField] private Transform premiumItempShopPos;

    [Header("PROP POSITIONS GROUPS")]
    [SerializeField] private GameObject lootPositionsGroups;

    private List<int> lootUsedPos = new List<int>();

    void Start()
    {
        StartCoroutine(SpawnLoot());
    }

    private IEnumerator SpawnLoot()
    {
        for (int i = 0; i < lootPos.Length; i++)
        {
            GameObject lootItem = SpawnNewItem();
            int randomLootPos = GenerateRandomPos();

            while (LootPosUsed(randomLootPos))
            {
                randomLootPos = GenerateRandomPos();
                yield return null;
            }

            if (lootItem != null)
            {
                GameObject clone = Instantiate(lootItem, lootPos[randomLootPos].position, lootItem.transform.rotation);

                clone.transform.parent = gameObject.transform.parent;

                lootUsedPos.Add(randomLootPos); 
            }
        }

        SpawnItemShop();

        ClearUnusedElements();
        StopCoroutine(SpawnLoot());
    }

    private bool LootPosUsed(int posToVerify)
    {
        return lootUsedPos.Contains(posToVerify);
    }

    void SpawnItemShop()
    {
        float probability = Random.Range(0, 100);

        probability *= GameManager.instance.premiumShopProbability;

        if (probability > 100)
        {
            Instantiate(nestMarket, premiumItempShopPos.position, premiumItempShopPos.transform.rotation);
        }
    }


    private int GenerateRandomPos()
    {
        int randomPosNumber = 0;

        return randomPosNumber = Random.Range(0, lootPos.Length);
    }

    private void ClearUnusedElements()
    {
        Destroy(lootPositionsGroups);
        Destroy(this.gameObject);
    }

    private GameObject SpawnNewItem()
    {
        GameObject itemToSpawn = null;

        //Dependiendo del resultado del random, se aumentara o bajara la probabilidad de que se spawnee el item que se elija.
        int randomItemSelection = Random.Range(0, 2);

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

                    if (NumberIsPair(lootProbability))
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
