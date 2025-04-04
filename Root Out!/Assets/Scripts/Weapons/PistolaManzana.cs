
using UnityEngine;
using Weapons;

public class PistolaManzana : WeaponsBase, IInteractable
{
    [Header("PistolaManzana Settings")]
    [SerializeField] private int bulletsPerShot = 6; // Número de balas por disparo
    [SerializeField] private WeaponData weaponData; // Referencia al Scriptable Object del arma

    private WeaponHandler weaponHandler; // Referencia al WeaponHandler

    protected override void Start()
    {
        base.Start();
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
    }

    protected override void Shoot()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Verificar si el arma está en el WeaponHandler y es el arma actual
        {
            FireBullet(bulletsPerShot); // Dispara 6 balas a la vez


            base.Shoot();
            AudioManager.instance.PlaySFX("Disparo Manzana");
        }
    }

    protected override void Reload()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject)
        {
            base.Reload();
        }
    }

    public void OnInteract()
    {
        if (GameManager.instance.playerInventoryHandler.Inventory.Contains(inventoryItemToAdd))
        {
            Destroy(gameObject);
        }
        else
        {
            AddWeaponToInventory();
            SetNewAimState();

            if (weaponHandler != null)
            {
                weaponHandler.PickUpWeapon(gameObject, weaponData); // Añade el arma al WeaponHandler
                transform.SetParent(weaponHandler.weaponHolder); // Asigna el transform del arma como hijo del weaponHolder
                transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posición local y la rotación local
                ActivateBulletText(); // Activar el texto de munición al recoger el arma
            }
        }
    }
}

