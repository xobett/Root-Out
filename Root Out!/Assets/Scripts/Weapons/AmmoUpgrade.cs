using UnityEngine;
using Weapons;

public class AmmoUpgrade : MonoBehaviour, IInteractable
{
    WeaponHandler weaponHandler;

    private void Start()
    {
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
    }

    public void OnInteract()
    {
        if (weaponHandler.weapons.Count > 0)
        {
            foreach (GameObject weapon in weaponHandler.weapons)
            {
                if (weapon.TryGetComponent<WeaponsBase>(out WeaponsBase weaponBase))
                {
                    weaponBase.currentAmmo = weaponBase.maxAmmo; // Establecer la munición actual al máximo  
                    weaponBase.bulletReserve = weaponBase.maxBulletReserve;
                    Debug.Log("Upgrade Adquirido");
                }
            }
        }
        Destroy(gameObject);
    }
}
