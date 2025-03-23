using System.Collections;
using TMPro;
using UnityEngine;
using Weapons;
public class PistolaCalabaza : WeaponsBase, IInteractable
{
    [Header("SHOTGUN SETTINGS")]
    [SerializeField] private int bulletsPerShot = 6; // N�mero de balas por disparo
    [SerializeField] private WeaponData weaponData; // Referencia al Scriptable Object del arma
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas

    private GameObject canvasRecarga;
    private Animation animacionRecarga;
    private WeaponHandler weaponHandler; // Referencia al WeaponHandler
    protected override void Start()
    {
        base.Start();
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
        canvasRecarga = GameObject.Find("Recarga");
        animacionRecarga = canvasRecarga.GetComponent<Animation>();

        canvasRecarga.SetActive(false); // Desactivar la imagen de recarga al inicio

        if (bulletText != null)
        {
            bulletText.gameObject.SetActive(false); // Desactivar el texto de munici�n al inicio
        }
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
            weaponHandler.PickUpWeapon(gameObject, weaponData); // A�ade el arma al WeaponHandler
            transform.SetParent(weaponHandler.weaponHolder); // Asigna el transform del arma como hijo del weaponHolder
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posici�n local y la rotaci�n local
        }
    }

    protected override void Shoot()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Verificar si el arma est� en el WeaponHandler y es el arma actual
        {
            FireBullet(bulletsPerShot); // Dispara 6 balas a la vez
            base.Shoot();
        }
        else
        {
            Debug.LogWarning("Weapon is not in the WeaponHandler or is not the current weapon.");
        }
    }

    protected override void Reload()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject)
        {
            base.Reload();
        }
    }

    protected override IEnumerator ReloadCoroutine()
    {
        if (canvasRecarga != null)
        {
            canvasRecarga.SetActive(true); // Activar la imagen de recarga
        }
        if (animacionRecarga != null)
        {
            animacionRecarga.Play(); // Reproducir la animaci�n de recarga
        }

        yield return new WaitForSeconds(reloadTime); // Espera el tiempo de recarga

        if (animacionRecarga != null)
        {
            animacionRecarga.Stop(); // Detener la animaci�n de recarga
        }
        if (canvasRecarga != null)
        {
            canvasRecarga.SetActive(false); // Desactivar la imagen de recarga
        }

        StartCoroutine(base.ReloadCoroutine());
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
