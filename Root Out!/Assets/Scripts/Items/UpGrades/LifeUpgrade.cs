using UnityEngine;

public class LifeUpgrade : MonoBehaviour, IInteractable
{

    [SerializeField] PlayerHealth scriptPlayerHealth;
    [SerializeField] InteractionHandler scriptInteractionHandler;
    //[SerializeField] PerksData _lifeUpgrade;
    [SerializeField] private float lifeUpgrade;

    public void OnInteract()
    {
        scriptPlayerHealth.currentHealth += lifeUpgrade;
        Destroy(gameObject);
    }
}
