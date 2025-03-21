using Unity.VisualScripting;
using UnityEngine;

public class SeedCoin : MonoBehaviour, IInteractable
{
    [SerializeField] private int coinsToAdd;
    public void OnInteract()
    {
        AddCoin();
    }

    private void AddCoin()
    {
        var playerInventory = GameManager.instance.playerInventoryHandler;
        playerInventory.seedCoins += coinsToAdd;

        Destroy(gameObject);
    }
}
