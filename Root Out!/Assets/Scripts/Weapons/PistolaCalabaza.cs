
using UnityEngine;
using Weapons;
public class PistolaCalabaza : WeaponsBase, IInteractable
{
    [Header("SHOTGUN SETTINGS")]
    [SerializeField] private WeaponData weaponData; // Referencia al Scriptable Object del arma

    private WeaponHandler weaponHandler; // Referencia al WeaponHandler
    protected override void Start()
    {
        base.Start();
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
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

    protected override void Shoot()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Verificar si el arma está en el WeaponHandler y es el arma actual
        {
            base.Shoot();
            AudioManagerSFX.Instance.PlaySFX("Pistola Calabaza");
        }
    }

    protected override void Reload()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject)
        {
            base.Reload();
        }
    }
}
