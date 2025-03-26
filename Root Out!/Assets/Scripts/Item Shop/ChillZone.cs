using UnityEngine;

public class ChillZone : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        RestoreHealth();
    }

    private void RestoreHealth()
    {
        var playerhealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        if (playerhealth.currentHealth < playerhealth.maxHealth)
        {
            playerhealth.SetPlayerHealth(playerhealth.maxHealth);
        }
    }
}
