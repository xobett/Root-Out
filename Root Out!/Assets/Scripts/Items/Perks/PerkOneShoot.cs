using UnityEngine;
using Weapons;

public class PerkOneShoot : MonoBehaviour, IInteractable
{
    private WeaponsBase weaponBase;

    void Start()
    {
        weaponBase = FindFirstObjectByType<WeaponsBase>();
    }

    public void OnInteract()
    {
        weaponBase.StartDamageIncreaseRoutine(); // Iniciar la corrutina en WeaponsBase
        Destroy(gameObject); // Destruye el objeto de mejora
    }
}
