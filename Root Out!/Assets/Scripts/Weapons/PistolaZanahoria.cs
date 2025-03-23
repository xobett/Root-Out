using System.Collections;
using TMPro;
using UnityEngine;
using Weapons;

public class PistolaZanahoria : WeaponsBase, IInteractable
{
    [Header("Pistola Zanahoria")]
    [SerializeField] private TextMeshProUGUI bulletText; // Referencia al componente de texto en el canvas
    [SerializeField] private WeaponData weaponData; // Referencia al Scriptable Object del arma

    private WeaponHandler weaponHandler; // Referencia al WeaponHandler
    private GameObject canvasRecarga;
    private Animation animacionRecarga;

    protected override void Start()
    {
        base.Start();
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
        canvasRecarga = GameObject.Find("Recarga");
        animacionRecarga = canvasRecarga.GetComponent<Animation>();

        canvasRecarga.SetActive(false); // Desactivar la imagen de recarga al inicio

        if (bulletText != null)
        {
            bulletText.gameObject.SetActive(false); // Desactivar el texto de munición al inicio
        }
    }
    protected override void Update()
    {
        base.Update();
        UpdateAmmoText(); // Actualiza el texto de munición 
    }

    protected override void Shoot()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Verificar si el arma está en el WeaponHandler y es el arma actual
        {
            base.Shoot();
            // AudioManager.instance.PlaySFX("Pistola Guisantes"); // Llamar al método PlaySFX en la instancia de AudioManager

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
            animacionRecarga.Play(); // Reproducir la animación de recarga
        }

        yield return new WaitForSeconds(reloadTime); // Espera el tiempo de recarga

        if (animacionRecarga != null)
        {
            animacionRecarga.Stop(); // Detener la animación de recarga
        }
        if (canvasRecarga != null)
        {
            canvasRecarga.SetActive(false); // Desactivar la imagen de recarga
        }

        StartCoroutine(base.ReloadCoroutine());
    }

    public void OnInteract()
    {
        if (weaponHandler != null)
        {
            weaponHandler.PickUpWeapon(gameObject, weaponData); // Añade el arma al WeaponHandler
            transform.SetParent(weaponHandler.weaponHolder); // Asigna el transform del arma como hijo del weaponHolder
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posición local y la rotación local
        }
    }

    private void UpdateAmmoText() // Actualiza el texto de munición
    {
        if (bulletText != null)
        {
            bulletText.text = $"{currentAmmo} / {bulletReserve}"; // Actualiza el texto con la munición actual y máxima
        }
        else
        {
            Debug.LogWarning("Ammo text component is not assigned.");
        }
    }
}


