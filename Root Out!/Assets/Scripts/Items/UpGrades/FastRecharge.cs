using UnityEngine;

public class FastRecharge : MonoBehaviour
{
    [Header("Ref Scripts")]
    [SerializeField] InteractionHandler interactionHandler;
    [SerializeField] Rifle rifle;
    [SerializeField] Revolver revolver;

    [Header("Upgrades")]
    [SerializeField] float rechargeUpgrade;

    void Update()
    {
        RechargeUpgrade();
    }

    private void RechargeUpgrade()
    {
      
    }
}


