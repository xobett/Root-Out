using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private int coinsToAdd = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Seed Coin"))
        {
            var playerInventory = GameManager.instance.playerInventoryHandler;
            playerInventory.seedCoins += coinsToAdd;

            Destroy(other.gameObject);
        }
    }
}
