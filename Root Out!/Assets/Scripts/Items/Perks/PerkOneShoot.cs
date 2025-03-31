using UnityEngine;
using Weapons;

public class PerkOneShoot : MonoBehaviour, IInteractable
{
    [Header("INVENTORY ITEM SETTINGS")]
    [SerializeField] private InventoryItemData inventoryItemToAdd;

    private WeaponsBase weaponBase;

    public void OnInteract()
    {
        foreach (GameObject weaponObject in GameObject.FindGameObjectsWithTag("Weapon"))
        {
            weaponBase = weaponObject.GetComponent<WeaponsBase>();
            weaponBase.StartDamageIncreaseRoutine(); // Iniciar la corrutina en WeaponsBase
            Debug.Log("One Shoot Activated: " + weaponBase.damage);
        }

        var playerInventory = GameManager.instance.playerInventoryHandler;
        playerInventory.AddItem(inventoryItemToAdd);

        Destroy(gameObject); // Destruye el objeto de mejora
    }
}
