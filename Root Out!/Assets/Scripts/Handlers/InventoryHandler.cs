using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    //Inventario de jugador
    [SerializeField] private List<InventoryItemData> inventory = new List<InventoryItemData>();
    [SerializeField] private InventoryItemData equippedItem;

    public List<InventoryItemData> Inventory => inventory;

    public void AddItem(InventoryItemData itemToAdd)
    {
        inventory.Add(itemToAdd);
    }

    public void RemoveItem(InventoryItemData itemToRemove)
    {
        inventory.Remove(itemToRemove);
    }

    public bool HasItem(InventoryItemData itemToVerify)
    {
        return inventory.Contains(itemToVerify);
    }

    public void EquipItem(InventoryItemData itemToEquip)
    {
        equippedItem = itemToEquip;
    }
}
