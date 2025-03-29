using UnityEngine;

public class VelocityUpgrade : MonoBehaviour , IInteractable
{
    [Header("INVENTORY ITEM SETTINGS")]
    [SerializeField] private InventoryItemData inventoryItemToAdd;

    PlayerMovement playerMovement;

    [Header("Upgrades")]
    [SerializeField] float walkUpgrade;
    [SerializeField] float sprintUpgrade;

    private void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }
    public void OnInteract()
    {
        var playerInventory = GameManager.instance.playerInventoryHandler;
        playerInventory.AddItem(inventoryItemToAdd);

        playerMovement.walkSpeed += walkUpgrade;
        playerMovement.sprintSpeed += sprintUpgrade;
        Destroy(gameObject);
    }

}
