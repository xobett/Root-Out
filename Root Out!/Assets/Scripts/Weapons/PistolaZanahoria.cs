using TMPro;
using UnityEngine;
using Weapons;

public class PistolaZanahoria : WeaponsBase, IInteractable
{
    [Header("Pistola Zanahoria")]
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas
    WeaponHandler weaponHandler; // Referencia al WeaponHandler

    protected override void Start()
    {
        base.Start();
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
    }
    protected override void Update()
    {
        base.Update();
        UpdateAmmoText(); // Actualiza el texto de munici�n 
    }

    protected override void Shoot()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Verificar si el arma est� en el WeaponHandler y es el arma actual
        {
            base.Shoot();
            // AudioManager.instance.PlaySFX("Pistola Guisantes"); // Llamar al m�todo PlaySFX en la instancia de AudioManager

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

    public void OnInteract()
    {
        if (weaponHandler != null)
        {
            weaponHandler.PickUpWeapon(gameObject); // A�ade el arma al WeaponHandler
            transform.SetParent(weaponHandler.weaponHolder); // Asigna el transform del arma como hijo del weaponHolder
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posici�n local y la rotaci�n local
        }
        else
        {
            Debug.LogWarning("WeaponHandler not found in the scene.");
        }
    }

    private void UpdateAmmoText() // Actualiza el texto de munici�n
    {
        if (bulletText != null)
        {
            bulletText.text = $"{currentAmmo} / {bulletReserve}"; // Actualiza el texto con la munici�n actual y m�xima
        }
        else
        {
            Debug.LogWarning("Ammo text component is not assigned.");
        }
    }
}


