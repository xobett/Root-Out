using TMPro;
using UnityEngine;
using Weapons;

public class PistolaBellota : WeaponsBase, IInteractable
{
    [Header("Pistola Bellota")]
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas
    [SerializeField] private WeaponData weaponData; // Referencia al Scriptable Object del arma
    WeaponInfoDisplay weaponInfoDisplay; // Referencia al script de visualización de información del arma
    WeaponHandler weaponHandler; // Referencia al WeaponHandler

    protected override void Start()
    {
        base.Start();
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
        weaponInfoDisplay = FindFirstObjectByType<WeaponInfoDisplay>();
    }
    protected override void Update()
    {
        base.Update();
        UpdateAmmoText(); // Actualiza el texto de munición después de disparar
    }
    public void OnInteract()
    {
        if (weaponHandler != null)
        {
            weaponHandler.PickUpWeapon(gameObject); // Añade el arma al WeaponHandler
            transform.SetParent(weaponHandler.weaponHolder); // Asigna el transform del arma como hijo del weaponHolder
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posición local y la rotación local

            // Mostrar la imagen del arma en el canvas
            if (weaponInfoDisplay != null && weaponData != null)
            {
                weaponInfoDisplay.DisplayWeaponImage(weaponData.weaponIcon);
            }
            else
            {
                Debug.LogWarning("WeaponInfoDisplay or WeaponData is not assigned.");
            }
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
        }
        else
        {
            Debug.LogWarning("Weapon is not in the WeaponHandler or is not the current weapon.");
        }
    }

    protected override void ReloadCorotine()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject)
        {
            base.ReloadCorotine();
        }
    }

    private void UpdateAmmoText() // Actualiza el texto de munición
    {
        if (bulletText != null)
        {
            bulletText.text = $"{currentAmmo}/{bulletReserve}"; // Actualiza el texto con la munición actual y máxima
        }
        else
        {
            Debug.LogWarning("Ammo text component is not assigned.");
        }
    }
}

