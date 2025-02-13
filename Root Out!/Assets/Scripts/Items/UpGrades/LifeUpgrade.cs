using UnityEngine;

public class LifeUpgrade : MonoBehaviour
{

    [SerializeField] PlayerHealth scriptPlayerHealth;
    [SerializeField] InteractionHandler scriptInteractionHandler;
    //[SerializeField] PerksData _lifeUpgrade;
    [SerializeField] private float lifeUpgrade;


    
    private void Update()
    {
        UpgradeLife();
    }


    private void UpgradeLife()
    {
        if (scriptInteractionHandler.Interact())
        {
            scriptPlayerHealth.currentHealth += lifeUpgrade;
            Destroy(gameObject);
            
        }
    }


}
