using UnityEngine;
using Weapons;

public class Ammo : MonoBehaviour, IInteractable
{
    [SerializeField] private int ammoToAdd;

    public void OnInteract()
    {
        AddAmmo();
    }

    private void AddAmmo()
    {
        //var weaponHandler = GameObject.FindFirstObjectByType<WeaponHandler>();
        //weaponHandler.currentWeapon.GetComponent<WeaponsBase>().bulletReserve += ammoToAdd;

        // Accede al InventoryHandler para agregar munición
        GameManager.instance.playerInventoryHandler.AddAmmo(ammoToAdd);

        Destroy(gameObject);
    }
}
