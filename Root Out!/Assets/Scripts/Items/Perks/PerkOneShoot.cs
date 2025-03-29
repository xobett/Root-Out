using UnityEngine;
using Weapons;

public class PerkOneShoot : MonoBehaviour, IInteractable
{
    [Header("INVENTORY ITEM SETTINGS")]
    [SerializeField] private InventoryItemData inventoryItemToAdd;

    private WeaponsBase weaponBase;

    void Start()
    {
        weaponBase = FindFirstObjectByType<WeaponsBase>();
    }

    public void OnInteract()
    {
        var playerInventory = GameManager.instance.playerInventoryHandler;
        playerInventory.AddItem(inventoryItemToAdd);

        weaponBase.StartDamageIncreaseRoutine(); // Iniciar la corrutina en WeaponsBase
        Destroy(gameObject); // Destruye el objeto de mejora
    }
}
