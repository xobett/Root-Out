using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Seed Coin"))
        {
            var playerInventory = transform.parent.gameObject.GetComponent<InventoryHandler>();
            playerInventory.seedCoins++;

            Destroy(other.gameObject);
        }
    }
}
