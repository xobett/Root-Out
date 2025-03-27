using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("SELECTED ITEM SETTINGS")]
    [SerializeField] private GameObject selectedItemBackground;
    [SerializeField] private Image itemIcon;

    [Header("SELECTED CROP INFO SETTINGS")]
    [SerializeField] private GameObject selectedCropInfo;

    [SerializeField] private TextMeshProUGUI cropName;
    [SerializeField] private TextMeshProUGUI cropType;
    [SerializeField] private TextMeshProUGUI cropDamage;
    [SerializeField] private TextMeshProUGUI cropCooldown;
    [SerializeField] private TextMeshProUGUI cropDescription;


    [Header("SELECTED WEAPON INFO SETTINGS")]
    [SerializeField] private GameObject selectedWeaponInfo;

    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private TextMeshProUGUI weaponDamage;
    [SerializeField] private TextMeshProUGUI weaponMaxAmmo;
    [SerializeField] private TextMeshProUGUI weaponReloadTime;
    [SerializeField] private TextMeshProUGUI weaponDescription;

    [Header("SELECTED PERK INFO SETTINGS")]
    [SerializeField] private GameObject selectedPerkInfo;

    [SerializeField] private TextMeshProUGUI perkName;
    [SerializeField] private TextMeshProUGUI perkDescription;

    [SerializeField] private InventoryItemData currenItem;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowInfo()
    {
        selectedItemBackground.SetActive(true);

        switch (currenItem.ItemType)
        {
            case ItemType.Crop:
                {
                    selectedWeaponInfo.SetActive(false);
                    selectedPerkInfo.SetActive(false);

                    selectedCropInfo.SetActive(true);

                    break;
                }

            case ItemType.Weapon:
                {
                    selectedCropInfo.SetActive(false);
                    selectedPerkInfo.SetActive(false);

                    selectedWeaponInfo.SetActive(true);

                    break;
                }

            case ItemType.Perk:
                {
                    selectedCropInfo.SetActive(false);
                    selectedWeaponInfo.SetActive(false);

                    selectedPerkInfo.SetActive(true);

                    break;
                }
        }
    }

    public void UpdateSelectedItemInfo(InventoryItemData data)
    {
        currenItem = data;
        itemIcon.sprite = currenItem.ItemIcon;

        switch (currenItem.ItemType)
        {
            case ItemType.Crop:
                {
                    var currentCropInfo = data.CropData;

                    cropName.text = currentCropInfo.CropName;
                    cropType.text = currentCropInfo.Type.ToString();
                    cropDamage.text = currentCropInfo.CropDamage.ToString();
                    cropCooldown.text = currentCropInfo.CropCooldownTime.ToString();
                    cropDescription.text = currentCropInfo.CropDescription;

                    break;
                }

            case ItemType.Weapon:
                {
                    var currentWeaponInfo = data.WeaponData;

                    weaponName.text = currentWeaponInfo.WeaponName;
                    weaponDamage.text = currentWeaponInfo.WeaponDamage.ToString();
                    weaponMaxAmmo.text = currentWeaponInfo.WeaponMaxAmmo.ToString();
                    weaponReloadTime.text = currentWeaponInfo.WeaponReloadTime.ToString();
                    weaponDescription.text = currentWeaponInfo.WeaponDescription;

                    break;
                }

            case ItemType.Perk:
                {
                    var currentPerkInfo = data.PerksData;

                    perkName.text = currentPerkInfo.PerkName;
                    perkDescription.text = currentPerkInfo.PerkDescription;

                    break;
                }
        }

        ShowInfo();
    }
}
