using System.Collections;
using TMPro;
using UnityEngine;
using Weapons;

public class PistolaGuisantes : WeaponsBase, IInteractable
{
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas
    [SerializeField] private WeaponData weaponData; // Referencia al Scriptable Object del arma

    private WeaponHandler weaponHandler; // Referencia al WeaponHandler
    protected override void Start()
    {
        base.Start();
        weaponHandler = FindFirstObjectByType<WeaponHandler>();

        if (bulletText != null)
        {
            bulletText.gameObject.SetActive(false); // Desactivar el texto de munición al inicio
        }
    }

    protected override void Update()
    {
        base.Update();
        UpdateAmmoText(); // Actualiza el texto de munición después de disparar
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
        if (weaponHandler != null)
        {
            weaponHandler.PickUpWeapon(gameObject, weaponData); // Añade el arma al WeaponHandler
            transform.SetParent(weaponHandler.weaponHolder); // Asigna el transform del arma como hijo del weaponHolder
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posición local y la rotación local
            if (bulletText != null)
            {
                bulletText.gameObject.SetActive(true); // Activar el texto de munición al recoger el arma
            }
        }
    }

    private void UpdateAmmoText() // Actualiza el texto de munición
    {
        if (bulletText != null && bulletText.gameObject.activeSelf)
        {
            bulletText.text = "Infinity"; // Actualiza el texto con la munición actual y máxima
        }
       
    }
}
