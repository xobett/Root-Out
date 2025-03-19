using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    //Inventario de jugador
    [SerializeField] public int seedCoins;
    [SerializeField] private List<InventoryItemData> itemsInventory = new List<InventoryItemData>();
    [SerializeField] private InventoryItemData equippedItem;

    public List<InventoryItemData> Inventory => itemsInventory;

    public void AddItem(InventoryItemData itemToAdd)
    {
        itemsInventory.Add(itemToAdd);
    }

    public void RemoveItem(InventoryItemData itemToRemove)
    {
        itemsInventory.Remove(itemToRemove);
    }

    public bool HasItem(InventoryItemData itemToVerify)
    {
        return itemsInventory.Contains(itemToVerify);
    }
}
