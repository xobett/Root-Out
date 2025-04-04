using UnityEngine;
using Weapons;

public class ExplosionUpgrade : MonoBehaviour, IInteractable
{
    [Header("INVENTORY ITEM SETTINGS")]
    [SerializeField] private InventoryItemData inventoryItemToAdd;

    [SerializeField] private GameObject explosionPrefab;
    private WeaponsBase weapon;

    public void OnInteract()
    {
        foreach (GameObject weaponObject in GameObject.FindGameObjectsWithTag("Weapon"))
        {
            weapon = weaponObject.GetComponent<WeaponsBase>();
            weapon.explosionUpgradeActivated = true; // Activar la mejora de explosión
            weapon.explosivePrefab = explosionPrefab; // Asignar el prefab de la explosión
            Debug.Log("Explosion Upgrade Activated: " + weapon.damage);
        }

        GameManager.instance.explosionUpgradeActivated = true;

        var playerInventory = GameManager.instance.playerInventoryHandler;
        playerInventory.AddItem(inventoryItemToAdd);
        Destroy(gameObject); // Destruye el objeto de mejora
    }
}
