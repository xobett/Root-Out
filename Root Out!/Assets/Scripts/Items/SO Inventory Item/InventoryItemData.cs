using Mono.Cecil;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
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
}

public enum ItemType
{
    None, Crop, Weapon, Perk
}

public class TestClass
{
    string Name;
    int number;
    float myFloat;

    public TestClass()
    {
        string newName = Name;
        int newNumber = number;
        float newFloat = myFloat;
    }

    public string TestString { get; set; }

    public int MyInt { get; set; }

}

public class Testttt : MonoBehaviour
{
    public TestClass newClass = new TestClass();

    private void Start()
    {
        newClass.TestString = "Hola";
    }
}
