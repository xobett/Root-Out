using UnityEngine;

public class CropCard : MonoBehaviour, IInteractable
{
    [Header("CROP CARD SETTINGS")]
    [SerializeField] private CropData cropToAdd;
    [SerializeField] private InventoryItemData inventoryItemToAdd;

    public void OnInteract()
    {
        var cropHandler = GameManager.instance.playerCropHandler;
        var inventoryHandler = GameManager.instance.playerInventoryHandler;

        cropHandler.AddCrop(cropToAdd);
        inventoryHandler.AddItem(inventoryItemToAdd);

        Debug.Log($"{cropToAdd.CropName} was added to the crop handler.");
        Destroy(gameObject);
    }
}
