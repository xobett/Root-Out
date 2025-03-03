using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [Header("INVENTORY PANEL SETTINGS")]
    [SerializeField] private GameObject inventoryPanel;

    [Header("INVENTORY GRID SETTINGS")]
    [SerializeField] private GameObject cropInventoryGrid;
    [SerializeField] private GameObject weaponInventoryGrid;
    [SerializeField] private GameObject perkInventoryGrid;

    [Header("PLAYER INVENTORY SETTINGS")]
    [SerializeField] private InventoryHandler playerInventory;

    [SerializeField] private int itemsDisplayed;

    private bool isOpened;

    void Start()
    {
        GetPlayerInventory();
    }

    void Update()
    {
        if (IsOpening())
        {
            OpenInventory();
        }
    }

    private void OpenInventory()
    {
        isOpened = !isOpened;
        inventoryPanel.SetActive(isOpened);

        //foreach (InventoryItemData inventoryItem in playerInventory.Inventory)
        //{
        //    Debug.Log("Is entering");
        //    var image = new GameObject();
        //    image.AddComponent<Image>();

        //    switch (inventoryItem.ItemType)
        //    {
        //        case ItemType.Crop:
        //            {
        //                Instantiate(image, cropInventoryGrid.transform);
        //                break;
        //            }

        //        case ItemType.Weapon:
        //            {
        //                Instantiate(image, weaponInventoryGrid.transform);
        //                break;
        //            }

        //        case ItemType.Perk:
        //            {
        //                Instantiate(image, perkInventoryGrid.transform);
        //                break;
        //            }
        //    }
        //}

        for (int i = itemsDisplayed; i < playerInventory.Inventory.Count; i++)
        {
            var itemProt = new GameObject();
            itemProt.AddComponent<Image>();

            switch (playerInventory.Inventory[i].ItemType)
            {
                case ItemType.Crop:
                    {
                        Instantiate(itemProt, cropInventoryGrid.transform);
                        break;
                    }

                case ItemType.Weapon:
                    {
                        Instantiate(itemProt, weaponInventoryGrid.transform);
                        break;
                    }

                case ItemType.Perk:
                    {
                        Instantiate(itemProt, perkInventoryGrid.transform);
                        break;
                    }
            }

            itemsDisplayed++;
            Debug.Log($"Inventory item number : {i}");
        }
    }

    private bool IsOpening()
    {
        return Input.GetKeyDown(KeyCode.I);
    }

    private void GetPlayerInventory()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryHandler>();
    }
}
