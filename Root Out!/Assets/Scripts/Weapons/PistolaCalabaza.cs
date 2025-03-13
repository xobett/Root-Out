using TMPro;
using UnityEngine;
using Weapons;
public class PistolaCalabaza : WeaponsBase, IInteractable
{
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas
    [SerializeField] WeaponHandler weaponHandler; // Referencia al WeaponHandler


    public void OnInteract()
    {
        if (weaponHandler != null)
        {
            weaponHandler.PickUpWeapon(gameObject); // Añade el arma al WeaponHandler
            transform.SetParent(weaponHandler.weaponHolder); // Asigna el transform del arma como hijo del weaponHolder
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posición local y la rotación local
        }
        else
        {
            Debug.LogWarning("WeaponHandler not found in the scene.");
        }
    }

    protected override void Shoot()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Verificar si el arma está en el WeaponHandler y es el arma actual
        {
            base.Shoot();
            UpdateAmmoText(); // Actualiza el texto de munición después de disparar
        }
        else
        {
            Debug.LogWarning("Weapon is not in the WeaponHandler or is not the current weapon.");
        }
    }

    private void UpdateAmmoText() // Actualiza el texto de munición
    {
        if (bulletText != null)
        {
            bulletText.text = $"{currentAmmo}/{maxAmmo}"; // Actualiza el texto con la munición actual y máxima
        }
        else
        {
            Debug.LogWarning("Ammo text component is not assigned.");
        }
    }
}
