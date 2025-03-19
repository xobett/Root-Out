using UnityEngine;

public class CropCard : MonoBehaviour, IInteractable
{
    [Header("CROP CARD SETTINGS")]
    [SerializeField] private CropData cropToAdd;
    [SerializeField] private InventoryItemData inventoryItemToAdd;

    public void OnInteract()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        var cropHandler = player.GetComponent<CropHandler>();
        var inventoryHandler = player.GetComponent<InventoryHandler>();

        cropHandler.AddCrop(cropToAdd);
        inventoryHandler.AddItem(inventoryItemToAdd);

        Debug.Log($"{cropToAdd.CropName} was added to the crop handler.");
        Destroy(gameObject);
    }
}
