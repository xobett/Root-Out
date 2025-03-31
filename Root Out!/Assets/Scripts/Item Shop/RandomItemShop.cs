using UnityEngine;

public class RandomItemShop : MonoBehaviour, IInteractable
{
    [Header("SHOP ITEMS SETTINGS")]
    [SerializeField] private InventoryItemData[] shopItems;
    [SerializeField] private const int shopItemCost = 10;

    [Header("SPAWN SHOP ITEM SETTINGS")]
    [SerializeField] private Transform itemSpawnPosition;

    [Header("SHOP ANIMATOR SETTINGS")]
    [SerializeField] private Animator shopInfoAnimator;
    [SerializeField] private Animator shopCostAnimator;

    [Header("DETECTION SETTINGS")]
    [SerializeField] private bool playerIsNear;

    public void OnInteract()
    {
        Debug.Log("Interacting with shop");
        BuyRandomItem();
    }

    private void BuyRandomItem()
    {
        if (GameManager.instance.playerInventoryHandler.SeedCoins >= shopItemCost)
        {
            //Instantiate(GetRandomShopItem().ItemPrefab, itemSpawnPosition.position, Quaternion.identity);

            var playerInventory = GameManager.instance.playerInventoryHandler;

            playerInventory.PaySeedCoins(shopItemCost);

            playerInventory.AddItem(GetRandomShopItem());
        }
        else
        {
            GameManager.instance.DisplayMessage("You don't have enough Seeds to buy an item!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerIsNear = true;
        DisplayIntroAnimations();
    }

    private void OnTriggerExit(Collider other)
    {
        playerIsNear = false;
        DisplayOutroAnimations();
    }

    private InventoryItemData GetRandomShopItem()
    {
        int randomInventoryItemIndex = Random.Range(0, shopItems.Length);

        return shopItems[randomInventoryItemIndex];
    }

    private void DisplayIntroAnimations()
    {
        shopInfoAnimator.SetTrigger("Intro");
        shopCostAnimator.SetTrigger("Intro");
    }

    private void DisplayOutroAnimations()
    {
        shopInfoAnimator.SetTrigger("Outro");
        shopCostAnimator.SetTrigger("Outro");
    }
}
