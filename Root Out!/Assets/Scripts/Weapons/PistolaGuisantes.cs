
using UnityEngine;
using Weapons;

public class PistolaGuisantes : WeaponsBase, IInteractable
{
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
            base.Shoot();
            AudioManager.instance.PlaySFX("Pistola Guisantes"); // Llamar al método PlaySFX en la instancia de AudioManager
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

    protected override void UpdateAmmoText()
    {
        if (bulletText != null)
        {
            bulletText.text = " IFINITIE"; // Actualiza el texto con la munición actual y máxima
        }
    }
}
