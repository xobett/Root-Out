using Unity.VisualScripting;
using UnityEngine;

public class SeedCoin : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        AddCoin();
    }

    private void AddCoin()
    {
        var playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryHandler>();
        playerInventory.seedCoins++;

        Destroy(gameObject);
    }
}
