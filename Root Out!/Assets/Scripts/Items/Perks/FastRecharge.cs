using UnityEngine;
using Weapons;

public class FastRecharge : MonoBehaviour, IInteractable
{
    [Header("INVENTORY ITEM SETTINGS")]
    [SerializeField] private InventoryItemData inventoryItemToAdd;

    private WeaponsBase weaponBase;
    [SerializeField] float rechargeUpgrade;

    private void Start()
    {
        weaponBase = FindFirstObjectByType<WeaponsBase>();
    }

    public void OnInteract()
    {
        if (!GameManager.instance.playerInventoryHandler.Inventory.Contains(inventoryItemToAdd))
        {
            var playerInventory = GameManager.instance.playerInventoryHandler;
            playerInventory.AddItem(inventoryItemToAdd);

            if (weaponBase != null)
            {
                weaponBase.reloadTime = rechargeUpgrade;
            } 
        }
        Destroy(gameObject);
    }
}


