using UnityEngine;

public class RandomItemShop : MonoBehaviour
{
    [Header("SHOP ITEMS SETTINGS")]
    [SerializeField] private InventoryItemData[] shopItems;
    [SerializeField] private int shopItemCost;

    [Header("SPAWN SHOP ITEM SETTINGS")]
    [SerializeField] private Transform itemSpawnPosition;

    [Header("DETECTION SETTINGS")]
    [SerializeField] private bool playerIsNear;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BuyRandomItem()
    {
        if (GameManager.instance.playerInventoryHandler.SeedCoins >= shopItemCost)
        {
            Instantiate(GetRandomShopItem().ItemPrefab, itemSpawnPosition.position, Quaternion.identity);
        }
        else
        {
            GameManager.instance.DisplayMessage("You don't have enough Seeds to buy an item!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerIsNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerIsNear = false;
    }

    private InventoryItemData GetRandomShopItem()
    {
        int randomInventoryItemIndex = Random.Range(0, shopItems.Length);

        return shopItems[randomInventoryItemIndex];
    }
}
