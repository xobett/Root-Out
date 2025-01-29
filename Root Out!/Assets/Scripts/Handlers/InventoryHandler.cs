using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    public List<SOItem> inventory = new List<SOItem>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddItem(SOItem itemToAdd)
    {
        inventory.Add(itemToAdd);
    }

    public void RemoveItem(SOItem itemToRemove)
    {
        inventory.Remove(itemToRemove);
    }
}
