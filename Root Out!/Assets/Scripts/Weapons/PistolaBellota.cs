using TMPro;
using UnityEngine;
using Weapons;

public class PistolaBellota : WeaponsBase, IInteractable
{
    [Header("Pistola Bellota")]
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas
    [SerializeField] private WeaponData weaponData; // Referencia al Scriptable Object del arma
    WeaponInfoDisplay weaponInfoDisplay; // Referencia al script de visualizaci�n de informaci�n del arma
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
        UpdateAmmoText(); // Actualiza el texto de munici�n despu�s de disparar
    }
    public void OnInteract()
    {
        if (weaponHandler != null)
        {
            weaponHandler.PickUpWeapon(gameObject); // A�ade el arma al WeaponHandler
            transform.SetParent(weaponHandler.weaponHolder); // Asigna el transform del arma como hijo del weaponHolder
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posici�n local y la rotaci�n local

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
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Verificar si el arma est� en el WeaponHandler y es el arma actual
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

    private void UpdateAmmoText() // Actualiza el texto de munici�n
    {
        if (bulletText != null)
        {
            bulletText.text = $"{currentAmmo}/{bulletReserve}"; // Actualiza el texto con la munici�n actual y m�xima
        }
        else
        {
            Debug.LogWarning("Ammo text component is not assigned.");
        }
    }
}

