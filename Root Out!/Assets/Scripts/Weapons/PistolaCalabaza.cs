using TMPro;
using UnityEngine;
using Weapons;
public class PistolaCalabaza : WeaponsBase, IInteractable
{
    [Header("SHOTGUN SETTINGS")]
    [SerializeField] private int bulletsPerShot = 6; // Número de balas por disparo
    [SerializeField] private WeaponData weaponData; // Referencia al Scriptable Object del arma
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas

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
    public void OnInteract()
    {
        SetNewAimState();

        if (weaponHandler != null)
        {
            weaponHandler.PickUpWeapon(gameObject, weaponData); // Añade el arma al WeaponHandler
            transform.SetParent(weaponHandler.weaponHolder); // Asigna el transform del arma como hijo del weaponHolder
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posición local y la rotación local
        }
    }

    protected override void Shoot()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Verificar si el arma está en el WeaponHandler y es el arma actual
        {
            FireBullet(bulletsPerShot); // Dispara 6 balas a la vez
            base.Shoot();
        }
    }

    protected override void Reload()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject)
        {
            base.Reload();
        }
    }

    private void UpdateAmmoText() // Actualiza el texto de munición
    {
        if (bulletText != null)
        {
            bulletText.text = $"{currentAmmo}/{bulletReserve}"; // Actualiza el texto con la munición actual y máxima
        }
        //else
        //{
        //    Debug.LogWarning("Ammo text component is not assigned.");
        //}
    }
}
