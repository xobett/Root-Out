using UnityEngine;

public class ChillZone : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject interactVfx;
    [SerializeField] private Transform spawnVfxPos;

    [SerializeField] private GameObject[] vitalJuices = new GameObject[3];
    private int juiceUsed = 0;

    public void OnInteract()
    {
        RestoreHealth();
    }

    private void RestoreHealth()
    {
        var playerhealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        if (juiceUsed < 3)
        {
            if (playerhealth.currentHealth < playerhealth.maxHealth)
            {
                AudioManagerSFX.Instance.PlaySFX("Tomar jugo");

                Instantiate(interactVfx, spawnVfxPos.position, interactVfx.transform.rotation);
                playerhealth.SetPlayerHealth(playerhealth.maxHealth);

                Destroy(vitalJuices[juiceUsed]);
                juiceUsed++;
            } 
        }
    }
}
