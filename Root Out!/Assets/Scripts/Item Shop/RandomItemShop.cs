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

    [Header("SHOP ANIMATOR SETTINGS")]
    [SerializeField] private Animator shopInfoAnimator;
    [SerializeField] private Animator shopCostAnimator;

    public void OnInteract()
    {
        DisplayCooldownMessage();
        BuyRandomItem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && cooldownActive && playerIsNear)
        {
            GameManager.instance.DisplayMessage("Next hatch is being prepared!");
            Debug.Log("Test");
        }
    }

    void DisplayCooldownMessage()
    {
        if (cooldownActive)
        {
            GameManager.instance.DisplayMessage("Next hatch is being prepared!");
        }
    }

    private void BuyRandomItem()
    {
        LayerMask nonInteraction = LayerMask.NameToLayer("Default");

        if (GameManager.instance.playerInventoryHandler.SeedCoins >= shopItemCost)
        {
            var playerInventory = GameManager.instance.playerInventoryHandler;
            playerInventory.PaySeedCoins(shopItemCost);

            gameObject.layer = nonInteraction;

            Instantiate(GetRandomShopItem().ItemPrefab, itemSpawnPosition.position, Quaternion.identity);

            StartCoroutine(RandomShopCooldown());
        }
        else
        {
            GameManager.instance.DisplayMessage("You don't have enough coins!");
        }
    }

    private IEnumerator RandomShopCooldown()
    {
        yield return new WaitForSeconds(1);

        cooldownActive = true;

        yield return new WaitForSeconds(shopCooldownTime);

        LayerMask interaction = LayerMask.NameToLayer("Growth Selection");
        gameObject.layer = interaction;

        cooldownActive = false;
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
