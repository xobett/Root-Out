using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoDisplay : MonoBehaviour
{
    public GameObject weaponImagePrefab; // Prefab de la imagen del arma
    public Transform canvasCurrentWeapon; // Transform del canvas donde se instanciará la imagen
    private GameObject currentWeaponImage; // Referencia a la imagen del arma actual

    public void DisplayWeaponImage(Sprite weaponIcon)
    {
        // Destruir la imagen del arma actual si existe
        if (currentWeaponImage != null)
        {
            Destroy(currentWeaponImage);
        }

        // Instanciar la nueva imagen del arma
        currentWeaponImage = Instantiate(weaponImagePrefab, canvasCurrentWeapon);
        currentWeaponImage.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        Image imageComponent = currentWeaponImage.GetComponent<Image>();
        if (imageComponent != null)
        {
            imageComponent.sprite = weaponIcon;
        }
    }
}
