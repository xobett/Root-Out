using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class InventoryHandler : MonoBehaviour
{
    [Header("COINS AND AMMO")]
    [SerializeField] private int seedCoins;
    [SerializeField] private int ammo;

    public int SeedCoins => seedCoins;
    public int Ammo => ammo;

    [Header("ITEMS IN INVENTORY")]
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

    public void AddAmmo(int ammountToAdd)
    {
        ammo += ammountToAdd;
    }

    public void UseAmmo(int ammountToUse)
    {
        ammo -= ammountToUse;
    }

    public void AddSeedCoins(int ammountToAdd)
    {
        seedCoins += ammountToAdd;
    }

    public void PaySeedCoins(int ammountToPay)
    {
        seedCoins -= ammountToPay;
    }
}
