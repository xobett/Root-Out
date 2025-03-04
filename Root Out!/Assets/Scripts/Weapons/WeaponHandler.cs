using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private WeaponData[] weapons = new WeaponData[maxWeapons]; // Arreglo para almacenar las armas
    [SerializeField] private WeaponData currentWeapon; // Arma actual
    private const int maxWeapons = 3; // N�mero m�ximo de armas que se pueden almacenar
    private int weaponCount = 0; // Contador de armas recogidas

    private void Update()
    {
        WeaponChance();
    }

    void WeaponChance()
    {
        // Cambiar de arma usando las teclas 1, 2 y 3
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2);
        }
    }

    // M�todo para recoger un arma nueva
    public void PickUpWeapon(WeaponData newWeapon)
    {
        if (weaponCount < maxWeapons)
        {
            // A�adir el arma al arreglo si no se ha alcanzado el m�ximo
            weapons[weaponCount] = newWeapon;
            weaponCount++;
        }
        else
        {
            // Si ya se tiene el m�ximo de armas, reemplazar la m�s antigua
            for (int i = 1; i < maxWeapons; i++)
            {
                weapons[i - 1] = weapons[i];
            }
            weapons[maxWeapons - 1] = newWeapon;
        }
        currentWeapon = newWeapon; // Establecer el arma actual
        Debug.Log("Picked up weapon: " + newWeapon.name);
    }

    // M�todo para cambiar de arma
    private void SwitchWeapon(int index)
    {
        if (index < weaponCount)
        {
            currentWeapon = weapons[index]; // Establecer el arma actual seg�n el �ndice
            Debug.Log("Switched to weapon: " + currentWeapon.name);
        }
        else
        {
            Debug.LogWarning("No weapon in slot " + (index + 1));
        }
    }
}
