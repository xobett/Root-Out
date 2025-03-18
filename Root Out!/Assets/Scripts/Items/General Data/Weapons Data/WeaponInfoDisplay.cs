using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoDisplay : MonoBehaviour
{
    public Image weaponIconImage;

    public void DisplayWeaponImage(Sprite weaponIcon)
    {
        weaponIconImage.sprite = weaponIcon;
    }
}
