using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PremiumItemShop : MonoBehaviour
{
    [SerializeField] private InventoryItemData[] premiumItems;
    [SerializeField] private Transform[] spawnPositions;
    private List<InventoryItemData> usedItems = new List<InventoryItemData>();

    private bool itemsSpawned;

    void Start()
    {
        StartCoroutine(SpawnItems());
    }

    void Update()
    {
        ItemSelection();
    }

    private void ItemSelection()
    {
        if (!itemsSpawned) return;
        foreach (Transform spawnPosition in spawnPositions)
        {
            if (spawnPosition.transform.childCount == 0)
            {
                Debug.Log("Should autodestroy");
                ItemChosen();
            }
        }
    }

    private IEnumerator SpawnItems()
    {
        var playerInventory = GameManager.instance.playerInventoryHandler;

        for (int i = 0; i < spawnPositions.Length; i++)
        {
            InventoryItemData itemToSpawn = GetPremiumItem();

            while (usedItems.Contains(itemToSpawn) || (itemToSpawn.ItemType == ItemType.Weapon && playerInventory.Inventory.Contains(itemToSpawn)))
            {
                itemToSpawn = GetPremiumItem();
                yield return null;
            }

            GameObject itemSpawned = Instantiate(itemToSpawn.ItemPrefab, spawnPositions[i].position, Quaternion.identity);
            itemSpawned.transform.parent = spawnPositions[i].transform;

            usedItems.Add(itemToSpawn);
        }

        itemsSpawned = true;

        StopCoroutine(SpawnItems());
    }

    private void ItemChosen()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            Destroy(spawnPositions[i].gameObject);
        }

        this.enabled = false;
    }

    private InventoryItemData GetPremiumItem()
    {
        int randomInventoryItemIndex = Random.Range(0, premiumItems.Length);

        return premiumItems[randomInventoryItemIndex];
    }
}
