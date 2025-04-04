using System.Collections;
using UnityEngine;

public class RandomItemShop : MonoBehaviour, IInteractable
{
    [Header("SHOP ITEMS SETTINGS")]
    [SerializeField] private InventoryItemData[] shopItems;

    [SerializeField] private const int shopItemCost = 10;
    [SerializeField] private float shopCooldownTime;

    [SerializeField] private bool cooldownActive;
    private bool playerIsNear;

    [Header("SPAWN SHOP ITEM SETTINGS")]
    [SerializeField] private Transform itemSpawnPosition;
    [SerializeField] private GameObject spawnVfx;

    [Header("SHOP ANIMATOR SETTINGS")]
    [SerializeField] private Animator shopInfoAnimator;
    [SerializeField] private Animator shopCostAnimator;

    [SerializeField] private Animator eggAnimator;

    public void OnInteract()
    {
        BuyRandomItem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && cooldownActive && playerIsNear)
        {
            GameManager.instance.DisplayMessage("Next hatch is being prepared!");
        }
    }

    private void BuyRandomItem()
    {
        if (GameManager.instance.playerInventoryHandler.SeedCoins >= shopItemCost)
        {
            StartCoroutine(SpawnRandomItem());
        }
        else
        {
            GameManager.instance.DisplayMessage("You don't have enough coins!");
        }
    }

    private IEnumerator SpawnRandomItem()
    {
        var playerInventory = GameManager.instance.playerInventoryHandler;
        playerInventory.PaySeedCoins(shopItemCost);

        LayerMask nonInteraction = LayerMask.NameToLayer("Default");
        gameObject.layer = nonInteraction;

        InventoryItemData itemToSpawn = GetRandomShopItem();

        while(itemToSpawn.ItemType == ItemType.Weapon && playerInventory.Inventory.Contains(itemToSpawn))
        {
            Debug.Log($"Inventory already contained {itemToSpawn.name}");
            itemToSpawn = GetRandomShopItem();
            yield return null;
        }

        eggAnimator.SetTrigger("Outro");

        Instantiate(spawnVfx, itemSpawnPosition.position, Quaternion.identity);
        Instantiate(GetRandomShopItem().ItemPrefab, itemSpawnPosition.position, Quaternion.identity);

        cooldownActive = true;

        yield return new WaitForSeconds(shopCooldownTime);

        eggAnimator.SetTrigger("Intro");

        yield return new WaitForSeconds(3);

        LayerMask interaction = LayerMask.NameToLayer("Growth Selection");
        gameObject.layer = interaction;

        cooldownActive = false;

        StopCoroutine(SpawnRandomItem());
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
