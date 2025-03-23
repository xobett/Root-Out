using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SeedCoin : MonoBehaviour, IInteractable
{
    [SerializeField] private int coinsToAdd;

    [SerializeField] private Image recarga;

    public void OnInteract()
    {
        AddCoin();
    }

    private void AddCoin()
    {
        var playerInventory = GameManager.instance.playerInventoryHandler;
        playerInventory.AddSeedCoins(coinsToAdd);

        Destroy(gameObject);
    }
}
