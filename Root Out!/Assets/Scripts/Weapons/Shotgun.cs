using TMPro;
using UnityEngine;
using Weapons;
public class Shotgun : WeaponsBase, IInteractable
{
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el 
    [SerializeField] WeaponHandler weaponHandler; // Referencia al WeaponHandler

    protected override void Start()
    {
        base.Start();
        UpdateAmmoText(); // Actualiza el texto de munici�n al inicio
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
            bulletText.text = $"{currentAmmo}/{maxAmmo}"; // Actualiza el texto con la munici�n actual y m�xima
        }
        else
        {
            Debug.LogWarning("Ammo text component is not assigned.");
        }
    }



}

