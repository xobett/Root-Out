using Unity.VisualScripting;
using UnityEngine;

public class UiInventoryIconInfo : MonoBehaviour
{
    [SerializeField] private CropItemInfo cropInfo;
    [SerializeField] private WeaponItemInfo weaponInfo;
    [SerializeField] private PerkItemInfo perkInfo;

    public void GetItemInfo(InventoryItemData data)
    {
        switch (data.ItemType)
        {
            case ItemType.Crop:
                {
                    var currentCropInfo = data.CropData;

                    cropInfo.cropName = currentCropInfo.CropName;
                    cropInfo.cropDescription = currentCropInfo.CropDescription;
                    cropInfo.typeOfCrop = currentCropInfo.Type.ToString();
                    cropInfo.cropCooldownTime = currentCropInfo.CropCooldownTime.ToString();
                    cropInfo.cropDamage = currentCropInfo.CropDamage.ToString();

                    break;
                }

            case ItemType.Weapon:
                {
                    var currentWeaponInfo = data.WeaponData;

                    weaponInfo.weaponName = currentWeaponInfo.WeaponName;
                    weaponInfo.weaponDescription = currentWeaponInfo.WeaponDescription;
                    weaponInfo.weaponType = currentWeaponInfo.WeaponType.ToString();
                    weaponInfo.weaponDamage = currentWeaponInfo.WeaponDamage.ToString();
                    weaponInfo.weaponMaxAmmo = currentWeaponInfo.WeaponMaxAmmo.ToString();

                    break;
                }

            case ItemType.Perk:
            {
                    var currentPerkInfo = data.PerksData;

                    perkInfo.perkName = currentPerkInfo.PerkName;
                    perkInfo.perkDescription = currentPerkInfo.PerkDescription;

                    break;
            }
        }
    }

}


[System.Serializable]
public struct CropItemInfo
{
    public string cropName;
    public string cropDescription;
    public string typeOfCrop;

    public string cropCooldownTime;
    public string cropDamage;
}

[System.Serializable]
public struct WeaponItemInfo
{
    public string weaponName;
    public string weaponDescription;
    public string weaponType;

    public string weaponDamage;

    public string reloadTime;
    public string weaponMaxAmmo;
}

[System.Serializable]
public struct PerkItemInfo
{
    public string perkName;
    public string perkDescription;
}
