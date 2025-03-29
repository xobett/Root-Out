using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private int coinsToAdd = 10;

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        if (tag == "Seed Coin" || tag == "Weapon" || tag == "Perk" || tag == "Crop Card")
        {
            other.GetComponent<Item>().ItemInteraction();
        }

    }
}
