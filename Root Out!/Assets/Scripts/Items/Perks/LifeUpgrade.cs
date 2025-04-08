using UnityEngine;

public class LifeUpgrade : MonoBehaviour, IInteractable
{

    PlayerHealth scriptPlayerHealth;
    [SerializeField] private float lifeUpgrade;

    private void Start()
    {
        scriptPlayerHealth = FindFirstObjectByType<PlayerHealth>();
    }
    public void OnInteract()
    {
        scriptPlayerHealth.currentHealth += lifeUpgrade;
        scriptPlayerHealth.SetPlayerHealth(lifeUpgrade);
        Destroy(gameObject);
    }
}
