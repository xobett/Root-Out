using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponPrefabs = new GameObject[3]; // Arreglo para almacenar los prefabs de las armas
    [SerializeField] private GameObject currentWeapon; // Arma actual
    [SerializeField] public Transform weaponHolder; // Transform donde se instanciará el arma
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
            SwitchWeapon(0); // Cambia al arma en el índice 0
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1); // Cambia al arma en el índice 1
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2); // Cambia al arma en el índice 2
        }
    }

    // Método para recoger un arma nueva
    public void PickUpWeapon(GameObject newWeaponPrefab) // Recibe el prefab del arma
    {
        if (weaponCount < 3)
        {
            // Añadir el arma al arreglo si no se ha alcanzado el máximo
            weaponPrefabs[weaponCount] = newWeaponPrefab; // Añadir el arma al arreglo
            weaponCount++; // Incrementar el contador de armas
        }
        else
        {
            // Si ya se tiene el máximo de armas, reemplazar la más antigua
            for (int i = 1; i < 3; i++) // Recorrer el arreglo de armas
            {
                weaponPrefabs[i - 1] = weaponPrefabs[i]; // Reemplazar el arma anterior
            }
            weaponPrefabs[3 - 1] = newWeaponPrefab; // Reemplazar la última arma
        }
        currentWeapon = newWeaponPrefab; // Establecer el arma actual
        Debug.Log("Picked up weapon: " + newWeaponPrefab.name);

        // Instanciar el prefab del arma en el Transform especificado
        if (weaponHolder != null && newWeaponPrefab != null) // Si el weaponHolder y el prefab del arma no estan vacios
        {
            GameObject newWeapon = Instantiate(newWeaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder); // Instanciar el arma
            SetCurrentWeapon(newWeapon); // Establecer el arma actual
        }
        else
        {
            Debug.LogWarning("Weapon holder or weapon prefab is not assigned.");
        }
    }

    // Método para cambiar de arma
    private void SwitchWeapon(int index)
    {
        if (index < weaponCount) // Si el índice es menor que el número de armas recogidas
        {
            SetCurrentWeapon(weaponPrefabs[index]); // Establecer el arma actual según el índice
            Debug.Log("Switched to weapon: " + currentWeapon.name); 
        }
        else
        {
            Debug.LogWarning("No weapon in slot " + (index + 1)); // Mostrar advertencia si no hay un arma en el índice
        }
    }

    // Método para establecer el arma actual y desactivar las otras armas
    private void SetCurrentWeapon(GameObject newWeapon)
    {
        if (currentWeapon != null) // Si hay un arma actual
        {
            currentWeapon.SetActive(false); // Desactivar el arma actual
        }
        currentWeapon = newWeapon; // Establecer la nueva arma
        currentWeapon.SetActive(true); // Activar la nueva arma
    }
}
