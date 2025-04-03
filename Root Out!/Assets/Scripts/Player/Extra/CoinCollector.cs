using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private int coinsToAdd = 10;
    [SerializeField] private GameObject grabVfx;

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        if (tag == "Seed Coin" || tag == "Weapon" || tag == "Perk" || tag == "Crop Card" || tag == "Ammo")
        {
            Instantiate(grabVfx, other.transform.position, grabVfx.transform.rotation);
            other.GetComponent<Item>().ItemInteraction();
        }

    }
}
