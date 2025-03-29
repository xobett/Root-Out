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
        var playerInventory = GameManager.instance.playerInventoryHandler;
        playerInventory.AddItem(inventoryItemToAdd);

        weaponBase.reloadTime = rechargeUpgrade;
        Destroy(gameObject);
    }



}


