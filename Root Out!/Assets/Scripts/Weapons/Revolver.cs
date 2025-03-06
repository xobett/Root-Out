using TMPro;
using UnityEngine;
using Weapons;
public class Revolver : WeaponsBase, IInteractable
{
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas
    [SerializeField] private Transform weaponLocation; // Referencia al transform del arma

    protected override void Start()
    {
        base.Start();
        UpdateAmmoText(); // Actualiza el texto de munición al inicio
    }
    public void OnInteract()
    {
        transform.SetParent(weaponLocation); // Asigna el transform del arma como padre
    }

    protected override void Shoot()
    {
        base.Shoot();
        UpdateAmmoText(); // Actualiza el texto de munición después de disparar
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
