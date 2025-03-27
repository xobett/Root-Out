using UnityEngine;
using Weapons;

public class FastRecharge : MonoBehaviour, IInteractable
{
    private WeaponsBase weaponBase;
    [SerializeField] float rechargeUpgrade;

    private void Start()
    {
        weaponBase = FindFirstObjectByType<WeaponsBase>();
    }

    public void OnInteract()
    {
        weaponBase.reloadTime = rechargeUpgrade;
        Destroy(gameObject);
    }



}


