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
                    weaponBase.currentAmmo = weaponBase.maxAmmo; // Establecer la munici�n actual al m�ximo  
                }
            }
        }
        Destroy(gameObject);
    }
}
