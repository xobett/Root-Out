
using UnityEngine;
using Weapons;

public class PistolaGuisantes : WeaponsBase, IInteractable
{
    [SerializeField] private WeaponData weaponData; // Referencia al Scriptable Object del arma
   // [SerializeField] private AudioSource audioSource; // Referencia al AudioSource para reproducir sonidos

    private WeaponHandler weaponHandler; // Referencia al WeaponHandler
    protected override void Start()
    {
        base.Start();
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
    }

    protected override void Shoot()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Verificar si el arma está en el WeaponHandler y es el arma actual
        {
            base.Shoot();
            AudioManagerSFX.Instance.PlaySFX("Disparo Guisantes"); // Reproducir el sonido de disparo

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
        if (GameManager.instance.playerInventoryHandler.Inventory.Contains(inventoryItemToAdd))
        {
            Destroy(gameObject);
        }
        else
        {
            AddWeaponToInventory();
            SetNewAimState();

            if (weaponHandler != null)
            {
                weaponHandler.PickUpWeapon(gameObject, weaponData); // Añade el arma al WeaponHandler
                transform.SetParent(weaponHandler.weaponHolder); // Asigna el transform del arma como hijo del weaponHolder
                transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posición local y la rotación local
                ActivateBulletText(); // Activar el texto de munición al recoger el arma
            }
        }
    }

    protected override void UpdateAmmoText()
    {
        if (bulletText != null)
        {
            bulletText.text = " IFINITIE"; // Actualiza el texto con la munición actual y máxima
        }
    }

    //private void PistolSound()
    //{
    //    if (Input.GetKey(KeyCode.Mouse0))
    //    {
    //        audioSource.Play(); // Reproducir el sonido de recarga

    //    }
    //}
}
