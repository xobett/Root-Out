using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponPrefabs = new GameObject[3]; // Arreglo para almacenar los prefabs de las armas
    [SerializeField] private GameObject currentWeapon; // Arma actual
    private int weaponCount = 0; // Contador de armas recogidas

    private void Update()
    {
        WeaponChance(); // Llama al método para cambiar de arma
    }

 
    void WeaponChance()
    {
        // Cambiar de arma usando las teclas 1, 2 y 3
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Switching to weapon 1");
            SwitchWeapon(0); // Cambia al arma en el índice 0
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Switching to weapon 2");
            SwitchWeapon(1); // Cambia al arma en el índice 1
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Switching to weapon 3");
            SwitchWeapon(2); // Cambia al arma en el índice 2
        }
    }

    // Método para recoger un arma nueva
    //public void PickUpWeapon(GameObject newWeaponPrefab)
    //{
    //    if (weaponCount < 3)
    //    {
    //        // Añadir el arma al arreglo si no se ha alcanzado el máximo
    //        weaponPrefabs[weaponCount] = newWeaponPrefab;
    //        weaponCount++;
    //    }
    //    //else
    //    //{
    //    //    // Si ya se tiene el máximo de armas, reemplazar la más antigua
    //    //    for (int i = 1; i < 3; i++)
    //    //    {
    //    //        weaponPrefabs[i - 1] = weaponPrefabs[i];
    //    //    }
    //    //    weaponPrefabs[3 - 1] = newWeaponPrefab;
    //    //}
    //    currentWeapon = newWeaponPrefab; // Establecer el arma actual
    //    Debug.Log("Picked up weapon: " + newWeaponPrefab.name);

    //}

    // Método para cambiar de arma
    private void SwitchWeapon(int index)
    {
        if (index < weaponCount)
        {
            currentWeapon = weaponPrefabs[index]; // Establecer el arma actual según el índice
            Debug.Log("Switched to weapon: " + currentWeapon.name);
        }
        else
        {
            Debug.LogWarning("No weapon in slot " + (index + 1));
        }
    }
}
