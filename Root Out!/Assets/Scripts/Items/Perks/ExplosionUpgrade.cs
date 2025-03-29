using UnityEngine;
using Weapons;

public class ExplosionUpgrade : MonoBehaviour, IInteractable
{
    [Header("INVENTORY ITEM SETTINGS")]
    [SerializeField] private InventoryItemData inventoryItemToAdd;

    [SerializeField] private GameObject explosionPrefab;
    private WeaponsBase weapon;

    private void Start()
    {
        weapon = FindFirstObjectByType<WeaponsBase>();
    }

    public void OnInteract()
    {
        if (weapon != null)
        {

            weapon.explosionUpgradeActivated = true; // Activar la mejora de explosión
            weapon.explosivePrefab = explosionPrefab; // Asignar el prefab de la explosión
            Debug.Log("Explosion Upgrade Activated: " + weapon.damage);
        }

        var playerInventory = GameManager.instance.playerInventoryHandler;
        playerInventory.AddItem(inventoryItemToAdd);
        Destroy(gameObject); // Destruye el objeto de mejora
    }
}
