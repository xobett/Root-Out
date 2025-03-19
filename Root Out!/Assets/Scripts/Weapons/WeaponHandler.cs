using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] public List<GameObject> weapons = new List<GameObject>(); // Lista para almacenar los prefabs de las armas
    [SerializeField] public GameObject currentWeapon; // Arma actual
    [SerializeField] public Transform weaponHolder; // Transform donde se instanciará el arma

    private void Update()
    {
        WeaponChance(); // Llama al método para cambiar de arma
    }

    // Cambiar de arma usando las teclas 1, 2 y 3
    void WeaponChance()
    {
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
    public void PickUpWeapon(GameObject newWeapon) // Recibe el GameObject del arma
    {
        weapons.Add(newWeapon); // Añadir el arma a la lista

        currentWeapon = newWeapon; // Establecer el arma actual
        Debug.Log("Picked up weapon: " + newWeapon.name);

        // Establecer el GameObject del arma en el Transform especificado
        if (weaponHolder != null && newWeapon != null) // Si el weaponHolder y el GameObject del arma no están vacíos
        {
            newWeapon.transform.SetParent(weaponHolder); // Establecer el padre del arma
            newWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posición local y la rotación local
            SetCurrentWeapon(newWeapon); // Establecer el arma actual
        }
        else
        {
            Debug.LogWarning("Weapon holder or weapon GameObject is not assigned.");
        }
    }

    // Método para cambiar de arma
    public void SwitchWeapon(int index)
    {
        if (index < weapons.Count) // Si el índice es menor que el número de armas recogidas
        {
            SetCurrentWeapon(weapons[index]); // Establecer el arma actual según el índice
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
        // Desactivar todas las armas antes de activar la nueva
        foreach (var weapon in weapons)
        {
            if (weapon != null) 
            {
                weapon.SetActive(false);
            }
        }

        currentWeapon = newWeapon; // Establecer la nueva arma
        currentWeapon.SetActive(true); // Activar la nueva arma
    }

    private void OnDrawGizmos()
    {
        if (weaponHolder != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(weaponHolder.position, weaponHolder.forward * 100); 
        }
    }
}
