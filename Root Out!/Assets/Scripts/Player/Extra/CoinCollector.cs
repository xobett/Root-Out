using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private int coinsToAdd = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Seed Coin"))
        {
            var playerInventory = transform.parent.gameObject.GetComponent<InventoryHandler>();
            playerInventory.seedCoins += coinsToAdd;

            Destroy(other.gameObject);
        }
    }
}
