using TMPro;
using UnityEngine;
using Weapons;

public class PistolaGuisantes : WeaponsBase, IInteractable
{
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

    protected override void Shoot()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Verificar si el arma est� en el WeaponHandler y es el arma actual
        {
            base.Shoot();
            AudioManager.instance.PlaySFX("Pistola Guisantes"); // Llamar al m�todo PlaySFX en la instancia de AudioManager
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
            weaponHandler.PickUpWeapon(gameObject, weaponData); // A�ade el arma al WeaponHandler
            transform.SetParent(weaponHandler.weaponHolder); // Asigna el transform del arma como hijo del weaponHolder
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posici�n local y la rotaci�n local
        }
    }

    private void UpdateAmmoText() // Actualiza el texto de munici�n
    {
        if (bulletText != null)
        {
            bulletText.text = "Infinity"; // Actualiza el texto con la munici�n actual y m�xima
        }
        else
        {
            Debug.LogWarning("Ammo text component is not assigned.");
        }
    }
}
