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
    // Transform donde se instanciar� el arma
    [SerializeField] public Transform weaponHolder;

    // Iconos en la rueda
    public List<Image> weaponIcons = new List<Image>();
    // Script que muestra la imagen del arma seleccionada
    public WeaponInfoDisplay weaponInfoDisplay;

    // �ndice de la selecci�n actual en la rueda de armas
    private int selectedWeaponIndex = 0;

    // Variable para rastrear el tiempo del �ltimo cambio de arma
    private float lastWeaponChangeTime = 0f;
    // Cooldown de 0.5 segundos
    private const float weaponChangeCooldown = 0.5f;

    private void Update()
    {
        // Maneja la rotaci�n de la rueda del rat�n para cambiar de arma
        HandleWeaponWheelRotation();
        // Actualiza los iconos de las armas
        UpdateWeaponIcons();
        // Maneja la selecci�n de arma al hacer clic
        HandleWeaponSelection();
    }

    // Maneja la rotaci�n de la rueda del rat�n para cambiar de arma
    private void HandleWeaponWheelRotation()
    {
        if (weapons.Count <= 1)
        {
            return; // No permitir rotaci�n si solo hay una arma
        }

        if (Time.time - lastWeaponChangeTime < weaponChangeCooldown)
        {
            return; // No permitir cambio de arma si el cooldown no ha terminado
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedWeaponIndex = (selectedWeaponIndex + 1) % weapons.Count;
            SwitchWeapon(selectedWeaponIndex);
            lastWeaponChangeTime = Time.time; // Actualizar el tiempo del �ltimo cambio de arma
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedWeaponIndex = (selectedWeaponIndex - 1 + weapons.Count) % weapons.Count;
            SwitchWeapon(selectedWeaponIndex);
            lastWeaponChangeTime = Time.time; // Actualizar el tiempo del �ltimo cambio de arma
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
                weaponIcons[i].color = (i == selectedWeaponIndex) ? Color.yellow : Color.white;
            }
            else
            {
                weaponIcons[i].enabled = false; // Desactivar iconos que no tienen un arma correspondiente
            }
        }
    }

    // Maneja la selecci�n de arma al hacer clic
    private void HandleWeaponSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectWeapon(selectedWeaponIndex);
        }
    }

    // M�todo para recoger un arma nueva
    public void PickUpWeapon(GameObject newWeapon, WeaponData newWeaponData) // Recibe el GameObject del arma y sus datos
    {
        weapons.Add(newWeapon); // A�adir el arma a la lista
        weaponDataList.Add(newWeaponData); // A�adir los datos del arma a la lista

        currentWeapon = newWeapon; // Establecer el arma actual
        Debug.Log("Picked up weapon: " + newWeapon.name);

        // Establecer el GameObject del arma en el Transform especificado
        if (weaponHolder != null && newWeapon != null) // Si el weaponHolder y el GameObject del arma no est�n vac�os
        {
            newWeapon.transform.SetParent(weaponHolder); // Establecer el padre del arma
            newWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posici�n local y la rotaci�n local
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
    }

    // M�todo para cambiar de arma
    public void SwitchWeapon(int index)
    {
        if (index < weapons.Count) // Verificar que el �ndice sea v�lido para todas las armas
        {
            SetCurrentWeapon(weapons[index]); // Establecer el arma actual seg�n el �ndice
            if (weaponInfoDisplay != null && index < weaponDataList.Count && weaponDataList[index] != null)
            {
                weaponInfoDisplay.DisplayWeaponImage(weaponDataList[index].weaponIcon); // Actualizar el icono del arma
            }
            Debug.Log("Switched to weapon: " + currentWeapon.name);
        }
        else
        {
            Debug.LogWarning("No weapon in slot " + (index + 1)); // Mostrar advertencia si no hay un arma en el �ndice
        }
    }

    // M�todo para establecer el arma actual y desactivar las otras armas
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

    // Seleccionar un arma seg�n el �ndice
    void SelectWeapon(int index)
    {
        if (IsValidIndex(index))
        {
            if (weaponInfoDisplay != null && index < weaponDataList.Count && weaponDataList[index] != null)
            {
                weaponInfoDisplay.DisplayWeaponImage(weaponDataList[index].weaponIcon);
            }
            SwitchWeapon(index);
        }
        else
        {
            Debug.LogWarning("WeaponsMenuHandler: �ndice fuera de los l�mites de los arrays.");
        }
    }

    // Verificar si el �ndice es v�lido
    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < weapons.Count;
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
