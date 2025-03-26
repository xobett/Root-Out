
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

    [Header("ITEM PREFAB")]
    [SerializeField] private GameObject itemPrefab;

    public string ItemName => itemName;
    public string ItemDescription => itemName;
    public ItemType ItemType => itemType;
    public Sprite ItemIcon => itemIcon;
    public CropData CropData => cropData;
    public WeaponData WeaponData => weaponData;
    public PerksData PerksData => perksData;
    public GameObject ItemPrefab => itemPrefab;
}

public enum ItemType
{
    None, Crop, Weapon, Perk
}
