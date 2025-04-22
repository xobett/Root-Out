using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using Weapons;

public class InventoryHandler : MonoBehaviour
{
    [Header("COINS AND AMMO")]
    [SerializeField] private int seedCoins;
    [SerializeField] private int ammo;
    [SerializeField] private int currentAmmoReserve; // Munición de reserva

    public int SeedCoins => seedCoins;
    public int Ammo => ammo;
    public int CurrentAmmoReserve => currentAmmoReserve;

    [Header("ITEMS IN INVENTORY")]
    [SerializeField] private List<InventoryItemData> itemsInventory = new List<InventoryItemData>();
    [SerializeField] private InventoryItemData equippedItem;
    private bool isReserveAmmoActive = false; // Controla si las reservas de munición están activas

    public List<InventoryItemData> Inventory => itemsInventory;
  
    public void AddItem(InventoryItemData itemToAdd)
    {
        itemsInventory.Add(itemToAdd);
    }

    public void AddAmmo(int ammountToAdd)
    {
        ammo += ammountToAdd;
    }

    public int UseAmmo(int amountToUse)
    {
        currentAmmoReserve -= amountToUse;
        return amountToUse;
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
