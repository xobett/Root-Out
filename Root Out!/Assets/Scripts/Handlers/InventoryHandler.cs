using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    //Inventario de jugador
    [SerializeField] private List<InventoryItemData> playerInventory = new List<InventoryItemData>();

    public void AddItem(InventoryItemData itemToAdd)
    {
        playerInventory.Add(itemToAdd);
    }

    public void RemoveItem(InventoryItemData itemToRemove)
    {
        playerInventory.Remove(itemToRemove);
    }

    public bool HasItem(InventoryItemData itemToVerify)
    {
        return playerInventory.Contains(itemToVerify);
    }
}
