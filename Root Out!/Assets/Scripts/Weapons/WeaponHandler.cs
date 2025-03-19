using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponHandler : MonoBehaviour
{
    // Lista para almacenar los prefabs de las armas
    [SerializeField] public List<GameObject> weapons = new List<GameObject>();
    // Lista para almacenar los datos de las armas
    [SerializeField] public List<WeaponData> weaponDataList = new List<WeaponData>();
    // Arma actual
    [SerializeField] public GameObject currentWeapon;
    // Transform donde se instanciará el arma
    [SerializeField] public Transform weaponHolder;

    // Iconos en la rueda
    [SerializeField] private List<Image> weaponIcons = new List<Image>();
    // Script que muestra la imagen del arma seleccionada
    [SerializeField] private WeaponInfoDisplay weaponInfoDisplay;
    // Canvas de la rueda de armas
    [SerializeField] private RectTransform weaponWheelCanvas;

    // Índice de la selección actual en la rueda de armas
    private int selectedWeaponIndex = 0;

    // Variable para rastrear el tiempo del último cambio de arma
    private float lastWeaponChangeTime = 0f;
    // Cooldown de 0.5 segundos
    private const float weaponChangeCooldown = 0.5f;
    // Velocidad de rotación del canvas
    [SerializeField] private float rotationSpeed = 100f;

    private void Update()
    {
        // Maneja la rotación de la rueda del ratón para cambiar de arma
        HandleWeaponWheelRotation();
        // Actualiza los iconos de las armas
        UpdateWeaponIcons();
        // Maneja la selección de arma al hacer clic
        HandleWeaponSelection();
        // Gira el canvas de la rueda de armas
        RotateWeaponWheelCanvas();
    }

    // Maneja la rotación de la rueda del ratón para cambiar de arma
    private void HandleWeaponWheelRotation()
    {
        if (weapons.Count <= 1)
        {
            return; // No permitir rotación si solo hay una arma
        }

        if (Time.time - lastWeaponChangeTime < weaponChangeCooldown)
        {
            return; // No permitir cambio de arma si el cooldown no ha terminado
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedWeaponIndex = (selectedWeaponIndex + 1) % weapons.Count;
            SwitchWeapon(selectedWeaponIndex);
            lastWeaponChangeTime = Time.time; // Actualizar el tiempo del último cambio de arma
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedWeaponIndex = (selectedWeaponIndex - 1 + weapons.Count) % weapons.Count;
            SwitchWeapon(selectedWeaponIndex);
            lastWeaponChangeTime = Time.time; // Actualizar el tiempo del último cambio de arma
        }
    }

    // Gira el canvas de la rueda de armas
    private void RotateWeaponWheelCanvas()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f && weaponWheelCanvas != null)
        {
            weaponWheelCanvas.Rotate(0f, 0f, scrollInput * rotationSpeed);
        }
    }

    // Actualiza los iconos de las armas
    private void UpdateWeaponIcons()
    {
        for (int i = 0; i < weaponIcons.Count; i++)
        {
            if (i < weaponDataList.Count)
            {
                weaponIcons[i].sprite = weaponDataList[i].weaponIcon;
                weaponIcons[i].enabled = true; // Asegurarse de que el icono esté habilitado
            }
            else
            {
                weaponIcons[i].enabled = false; // Desactivar iconos que no tienen un arma correspondiente
            }
        }
    }

    // Maneja la selección de arma al hacer clic
    private void HandleWeaponSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectWeapon(selectedWeaponIndex);
        }
    }

    // Método para recoger un arma nueva
    public void PickUpWeapon(GameObject newWeapon, WeaponData newWeaponData) // Recibe el GameObject del arma y sus datos
    {
        if (weapons.Count < 6) // Limitar el número de armas a 6
        {
            weapons.Add(newWeapon); // Añadir el arma a la lista
            weaponDataList.Add(newWeaponData); // Añadir los datos del arma a la lista

            currentWeapon = newWeapon; // Establecer el arma actual
            Debug.Log("Picked up weapon: " + newWeapon.name);

            // Establecer el GameObject del arma en el Transform especificado
            if (weaponHolder != null && newWeapon != null) // Si el weaponHolder y el GameObject del arma no están vacíos
            {
                newWeapon.transform.SetParent(weaponHolder); // Establecer el padre del arma
                newWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posición local y la rotación local
                SetCurrentWeapon(newWeapon); // Establecer el arma actual

                // Actualizar la imagen del arma en WeaponInfoDisplay
                int weaponDataIndex = weapons.IndexOf(newWeapon);
                if (weaponInfoDisplay != null && weaponDataIndex < weaponDataList.Count && weaponDataList[weaponDataIndex] != null)
                {
                    weaponInfoDisplay.DisplayWeaponImage(weaponDataList[weaponDataIndex].weaponIcon); // Actualizar el icono del arma
                }
            }
            else
            {
                Debug.LogWarning("Weapon holder or weapon GameObject is not assigned.");
            }

            // Actualizar los iconos de las armas
            UpdateWeaponIcons();
        }
        else
        {
            Debug.LogWarning("Cannot pick up more than 6 weapons.");
        }
    }

    // Método para cambiar de arma
    public void SwitchWeapon(int index)
    {
        if (index < weapons.Count) // Verificar que el índice sea válido para todas las armas
        {
            SetCurrentWeapon(weapons[index]); // Establecer el arma actual según el índice
            if (weaponInfoDisplay != null && index < weaponDataList.Count && weaponDataList[index] != null)
            {
                weaponInfoDisplay.DisplayWeaponImage(weaponDataList[index].weaponIcon); // Actualizar el icono del arma
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

    // Seleccionar un arma según el índice
    void SelectWeapon(int index) // Recibe el índice del arma
    {
        if (IsValidIndex(index)) // Verificar si el índice es válido
        {
            if (weaponInfoDisplay != null && index < weaponDataList.Count && weaponDataList[index] != null) // Actualizar el icono del arma
            {
                weaponInfoDisplay.DisplayWeaponImage(weaponDataList[index].weaponIcon);  // Actualizar el icono del arma
            }
            SwitchWeapon(index); // Cambiar al arma seleccionada
        }
        else
        {
            Debug.LogWarning("WeaponsMenuHandler: Índice fuera de los límites de los arrays.");
        }
    }

    // Verificar si el índice es válido
    private bool IsValidIndex(int index) // Recibe el índice del arma
    {
        return index >= 0 && index < weapons.Count; // Verificar si el índice está dentro de los límites de los arrays
    }

    // Dibujar gizmos en el editor para visualizar el weaponHolder
    private void OnDrawGizmos()
    {
        if (weaponHolder != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(weaponHolder.position, weaponHolder.forward * 100);
        }
    }
}
