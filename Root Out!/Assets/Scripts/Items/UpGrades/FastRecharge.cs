using UnityEngine;
using Weapons;

public class FastRecharge : MonoBehaviour
{
    [SerializeField] WeaponsBase weaponBase;
    [Header("Upgrades")]
    [SerializeField] float rechargeUpgrade;

    void Update()
    {
        RechargeUpgrade();
    }

    private void RechargeUpgrade()
    {
        weaponBase.reloadTime = rechargeUpgrade;
    }
    
}


