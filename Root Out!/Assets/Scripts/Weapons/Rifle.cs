using TMPro;
using UnityEngine;
using Weapons;

public class Rifle : WeaponsBase
{
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas
    [SerializeField] WeaponHandler weaponHandler; // Referencia al WeaponHandler

    protected override void Start()
    {
        base.Start();
        UpdateAmmoText(); // Actualiza el texto de munici�n al inicio
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

