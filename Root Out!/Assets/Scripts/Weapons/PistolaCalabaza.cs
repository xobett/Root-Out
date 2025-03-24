
using UnityEngine;
using Weapons;
public class PistolaCalabaza : WeaponsBase, IInteractable
{
    [Header("SHOTGUN SETTINGS")]
    [SerializeField] private int bulletsPerShot = 6; // N�mero de balas por disparo
    [SerializeField] private WeaponData weaponData; // Referencia al Scriptable Object del arma

    private WeaponHandler weaponHandler; // Referencia al WeaponHandler
    protected override void Start()
    {
        base.Start();
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
    }
    public void OnInteract()
    {
        SetNewAimState();

        if (weaponHandler != null)
        {
            weaponHandler.PickUpWeapon(gameObject, weaponData); // A�ade el arma al WeaponHandler
            transform.SetParent(weaponHandler.weaponHolder); // Asigna el transform del arma como hijo del weaponHolder
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posici�n local y la rotaci�n local
            ActivateBulletText(); // Activar el texto de munici�n al recoger el arma
        }
    }

    protected override void Shoot()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Verificar si el arma est� en el WeaponHandler y es el arma actual
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
}
