using TMPro;
using UnityEngine;
using Weapons;

public class Rifle : WeaponsBase
{
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas
    [SerializeField] WeaponHandler weaponHandler; // Referencia al WeaponHandler

    protected override void Shoot()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Verificar si el arma est� en el WeaponHandler y es el arma actual
        {
            base.Shoot();
            UpdateAmmoText(); // Actualiza el texto de munici�n despu�s de disparar
        }
        else
        {
            Debug.LogWarning("Weapon is not in the WeaponHandler or is not the current weapon.");
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

