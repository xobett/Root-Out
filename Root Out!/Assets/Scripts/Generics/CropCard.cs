using UnityEngine;

public class CropCard : MonoBehaviour, IInteractable
{
    [Header("CROP CARD SETTINGS")]
    [SerializeField] private CropData cropToAdd;
    [SerializeField] private InventoryItemData inventoryItemToAdd;

    public void OnInteract()
    {
        AddItemToInventory();
    }

    public void AddItemToInventory()
    {
        var cropHandler = GameManager.instance.playerCropHandler;
        var inventoryHandler = GameManager.instance.playerInventoryHandler;

        cropHandler.AddCrop(cropToAdd);
        inventoryHandler.AddItem(inventoryItemToAdd);
        Destroy(gameObject);
    }
}
