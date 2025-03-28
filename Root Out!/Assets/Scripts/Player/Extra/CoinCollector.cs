using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private int coinsToAdd = 10;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Item>().ItemInteraction();
    }
}
