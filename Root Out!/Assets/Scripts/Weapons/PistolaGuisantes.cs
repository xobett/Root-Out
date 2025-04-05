
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
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Verificar si el arma est� en el WeaponHandler y es el arma actual
        {
            base.Shoot();
            AudioManager.instance.PlaySFX("Pistola Guisantes"); // Llamar al m�todo PlaySFX en la instancia de AudioManager
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
                weaponHandler.PickUpWeapon(gameObject, weaponData); // A�ade el arma al WeaponHandler
                transform.SetParent(weaponHandler.weaponHolder); // Asigna el transform del arma como hijo del weaponHolder
                transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posici�n local y la rotaci�n local
                ActivateBulletText(); // Activar el texto de munici�n al recoger el arma
            }
        }
    }

    protected override void UpdateAmmoText()
    {
        if (bulletText != null)
        {
            bulletText.text = " IFINITIE"; // Actualiza el texto con la munici�n actual y m�xima
        }
    }
}
