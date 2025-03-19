using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponHandler : MonoBehaviour
{
    [SerializeField] public List<GameObject> weapons = new List<GameObject>(); // Lista para almacenar los prefabs de las armas
    [SerializeField] public GameObject currentWeapon; // Arma actual
    [SerializeField] public Transform weaponHolder; // Transform donde se instanciará el arma

    public Image[] weaponIcons; // Iconos en la rueda
    public WeaponData[] weaponDataArray; // Datos (por ejemplo, iconos ampliados) de las armas
    public WeaponInfoDisplay weaponInfoDisplay; // Script que muestra la imagen del arma seleccionada

    // Índice de la selección actual en la rueda de armas
    private int selectedWeaponIndex = 0;

    // Variable para rastrear el tiempo del último cambio de arma
    private float lastWeaponChangeTime = 0f;
    private const float weaponChangeCooldown = 0.5f; // Cooldown de 0.5 segundos

    // Lista para almacenar las dos últimas armas recogidas
    private List<GameObject> lastTwoWeapons = new List<GameObject>();

    private void Update()
    {
        WeaponChance(); // Llama al método para cambiar de arma
        HandleWeaponWheelRotation();
        UpdateWeaponIcons();
        HandleWeaponSelection();
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

    private void HandleWeaponWheelRotation()
    {
        if (lastTwoWeapons.Count <= 1)
        {
            return; // No permitir rotación si solo hay una arma
        }

        if (Time.time - lastWeaponChangeTime < weaponChangeCooldown)
        {
            return; // No permitir cambio de arma si el cooldown no ha terminado
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedWeaponIndex = (selectedWeaponIndex + 1) % lastTwoWeapons.Count;
            SwitchWeapon(selectedWeaponIndex);
            lastWeaponChangeTime = Time.time; // Actualizar el tiempo del último cambio de arma
        }
    }

    private void UpdateWeaponIcons()
    {
        for (int i = 0; i < weaponIcons.Length; i++)
        {
            weaponIcons[i].color = (i == selectedWeaponIndex) ? Color.yellow : Color.white;
        }
    }

    private void HandleWeaponSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectWeapon(selectedWeaponIndex);
        }
    }

    // Método para recoger un arma nueva
    public void PickUpWeapon(GameObject newWeapon) // Recibe el GameObject del arma
    {
        weapons.Add(newWeapon); // Añadir el arma a la lista

        currentWeapon = newWeapon; // Establecer el arma actual
        Debug.Log("Picked up weapon: " + newWeapon.name);

        // Actualizar la lista de las dos últimas armas recogidas
        if (lastTwoWeapons.Count == 2)
        {
            lastTwoWeapons.RemoveAt(0); // Eliminar la arma más antigua
        }
        lastTwoWeapons.Add(newWeapon); // Añadir la nueva arma

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
        if (index < lastTwoWeapons.Count) // Verificar que el índice sea válido para las dos últimas armas
        {
            SetCurrentWeapon(lastTwoWeapons[index]); // Establecer el arma actual según el índice
            int weaponDataIndex = weapons.IndexOf(lastTwoWeapons[index]);
            if (weaponInfoDisplay != null && weaponDataArray[weaponDataIndex] != null)
            {
                weaponInfoDisplay.DisplayWeaponImage(weaponDataArray[weaponDataIndex].weaponIcon); // Actualizar el icono del arma
            }
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

    void SelectWeapon(int index)
    {
        if (IsValidIndex(index))
        {
            int weaponDataIndex = weapons.IndexOf(lastTwoWeapons[index]);
            if (weaponInfoDisplay != null && weaponDataArray[weaponDataIndex] != null)
            {
                weaponInfoDisplay.DisplayWeaponImage(weaponDataArray[weaponDataIndex].weaponIcon);
            }
            SwitchWeapon(index);
        }
        else
        {
            Debug.LogWarning("WeaponsMenuHandler: Índice fuera de los límites de los arrays.");
        }
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < lastTwoWeapons.Count;
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
