using System;
using TMPro;
using UnityEngine;
using Weapons;
public class Revolver : WeaponsBase
{
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas

    protected override void Start()
    {
        base.Start();
        UpdateAmmoText(); // Actualiza el texto de munici�n al inicio
    }

    protected override void Shoot()
    {
        base.Shoot();
        UpdateAmmoText(); // Actualiza el texto de munici�n despu�s de disparar
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
