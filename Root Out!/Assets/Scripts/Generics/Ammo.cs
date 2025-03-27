using UnityEngine;

public class Ammo : MonoBehaviour, IInteractable
{
    [SerializeField] private int ammoToAdd;

    public void OnInteract()
    {
        AddAmmo();
    }

    private void AddAmmo()
    {
        var playerInventory = GameManager.instance.playerInventoryHandler;
        playerInventory.AddAmmo(ammoToAdd);

        Destroy(gameObject);
    }
}
