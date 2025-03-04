
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory Data/Create new Inventory Item Data")]
public class InventoryItemData : ScriptableObject
{
    [Header("GENERAL INFORMATION")]
    [SerializeField] private string itemName;

    [SerializeField] private string itemDescription;

    [SerializeField] private ItemType itemType = ItemType.None;

    [Header("VISUAL ICON")]
    [SerializeField] private Sprite itemIcon;

    [Header("ITEM DATA")]
    [SerializeField] private CropData cropData;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private PerksData perksData;

    public string ItemName => itemName;
    public string ItemDescription => itemName;
    public ItemType ItemType => itemType;
    public Sprite ItemIcon => itemIcon;
    public CropData CropData => CropData;
    public WeaponData WeaponData => weaponData;
    public PerksData PerksData => perksData;
}

public enum ItemType
{
    None, Crop, Weapon, Perk
}
