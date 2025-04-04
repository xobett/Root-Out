using UnityEngine;

public class ChillZone : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject interactVfx;
    [SerializeField] private Transform spawnVfxPos;

    public void OnInteract()
    {
        RestoreHealth();
    }

    private void RestoreHealth()
    {
        var playerhealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        if (playerhealth.currentHealth < playerhealth.maxHealth)
        {
            Instantiate(interactVfx, spawnVfxPos.position, interactVfx.transform.rotation);
            playerhealth.SetPlayerHealth(playerhealth.maxHealth);
        }
    }
}
