using TMPro;
using UnityEngine;
using Weapons;
public class Shotgun : WeaponsBase, IInteractable
{
    [Header("SHOTGUN SETTINGS")]
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas
    [SerializeField] WeaponHandler weaponHandler; // Referencia al WeaponHandler
    [SerializeField] private int bulletsPerShot = 6; // N�mero de balas por disparo

    protected override void Shoot()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Verificar si el arma est� en el WeaponHandler y es el arma actual
        {
            FireBullet(bulletsPerShot); // Dispara 6 balas a la vez
            UpdateAmmoText(); // Actualiza el texto de munici�n despu�s de disparar
            base.Shoot();
        }
        else
        {
            Debug.LogWarning("Weapon is not in the WeaponHandler or is not the current weapon.");
        }
    }

    public void OnInteract()
    {
        if (weaponHandler != null)
        {
            weaponHandler.PickUpWeapon(gameObject); // A�ade el arma al WeaponHandler
            transform.SetParent(weaponHandler.weaponHolder); // Asigna el transform del arma como hijo del weaponHolder
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("WeaponHandler not found in the scene.");
        }
    }

    private void UpdateAmmoText() // Actualiza el texto de munici�n
    {
        if (bulletText != null)
        {
            bulletText.text = $"{currentAmmo}/{maxAmmo}"; // Actualiza el texto con la munici�n actual y m�xima
        }
        else
        {
            Debug.LogWarning("Ammo text component is not assigned.");
        }
    }
}

