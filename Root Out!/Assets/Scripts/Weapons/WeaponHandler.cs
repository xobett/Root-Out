using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private WeaponData[] weapons = new WeaponData[maxWeapons]; // Arreglo para almacenar las armas
    [SerializeField] private WeaponData currentWeapon; // Arma actual
    private const int maxWeapons = 3; // Número máximo de armas que se pueden almacenar
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

    // Método para recoger un arma nueva
    public void PickUpWeapon(WeaponData newWeapon)
    {
        if (weaponCount < maxWeapons)
        {
            // Añadir el arma al arreglo si no se ha alcanzado el máximo
            weapons[weaponCount] = newWeapon;
            weaponCount++;
        }
        else
        {
            // Si ya se tiene el máximo de armas, reemplazar la más antigua
            for (int i = 1; i < maxWeapons; i++)
            {
                weapons[i - 1] = weapons[i];
            }
            weapons[maxWeapons - 1] = newWeapon;
        }
        currentWeapon = newWeapon; // Establecer el arma actual
        Debug.Log("Picked up weapon: " + newWeapon.name);
    }

    // Método para cambiar de arma
    private void SwitchWeapon(int index)
    {
        if (index < weaponCount)
        {
            currentWeapon = weapons[index]; // Establecer el arma actual según el índice
            Debug.Log("Switched to weapon: " + currentWeapon.name);
        }
        else
        {
            Debug.LogWarning("No weapon in slot " + (index + 1));
        }
    }
}
