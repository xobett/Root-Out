using UnityEngine;

public class PremiumItemShop : MonoBehaviour, IInteractable
{
    [SerializeField] private InventoryItemData[] premiumItems;
    [SerializeField] private Transform[] spawnPositions;

    void Start()
    {
        SpawnItems();
    }

    void Update()
    {
        
    }

    private void SpawnItems()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            Instantiate(GetPremiumItem().ItemPrefab, spawnPositions[i].position, Quaternion.identity);
        }
    }

    public void OnInteract()
    {
        Debug.Log("Testing");
    }

    private InventoryItemData GetPremiumItem()
    {
        int randomInventoryItemIndex = Random.Range(0, premiumItems.Length);

        return premiumItems[randomInventoryItemIndex];
    }
}
