using TMPro;
using UnityEngine;
using Weapons;

public class Rifle : WeaponsBase
{
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas

    protected override void Shoot()
    {
        base.Shoot();
        UpdateAmmoText();
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

