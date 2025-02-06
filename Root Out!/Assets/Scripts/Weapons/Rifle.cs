using UnityEngine;
using Weapons;
using TMPro;

public class Rifle : WeaponsBase
{
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas
    
    protected override void Shoot()
    {
        base.Shoot();
        UpdateAmmoText(); // Actualiza el texto de munici�n despu�s de disparar
       // CameraShake.StartShake(0.1f, 0.1f);
       // AudioManager.instance.PlaySFX("Automatic Rifle"); // Reproduce el sonido de disparo
  
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

