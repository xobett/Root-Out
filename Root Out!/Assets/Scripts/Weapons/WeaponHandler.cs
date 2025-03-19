using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponHandler : MonoBehaviour
{
    [SerializeField] public List<GameObject> weapons = new List<GameObject>(); // Lista para almacenar los prefabs de las armas
    [SerializeField] public GameObject currentWeapon; // Arma actual
    [SerializeField] public Transform weaponHolder; // Transform donde se instanciar� el arma

    public Image[] weaponIcons; // Iconos en la rueda
    public WeaponData[] weaponDataArray; // Datos (por ejemplo, iconos ampliados) de las armas
    public WeaponInfoDisplay weaponInfoDisplay; // Script que muestra la imagen del arma seleccionada

    // �ndice de la selecci�n actual en la rueda de armas
    private int selectedWeaponIndex = 0;

    // Variable para rastrear el tiempo del �ltimo cambio de arma
    private float lastWeaponChangeTime = 0f;
    private const float weaponChangeCooldown = 0.5f; // Cooldown de 0.5 segundos

    // Lista para almacenar las dos �ltimas armas recogidas
    private List<GameObject> lastTwoWeapons = new List<GameObject>();

    private void Update()
    {
        WeaponChance(); // Llama al m�todo para cambiar de arma
        HandleWeaponWheelRotation();
        UpdateWeaponIcons();
        HandleWeaponSelection();
    }

    // Cambiar de arma usando las teclas 1, 2 y 3
    void WeaponChance()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0); // Cambia al arma en el �ndice 0
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1); // Cambia al arma en el �ndice 1
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2); // Cambia al arma en el �ndice 2
        }
    }

    private void HandleWeaponWheelRotation()
    {
        if (lastTwoWeapons.Count <= 1)
        {
            return; // No permitir rotaci�n si solo hay una arma
        }

        if (Time.time - lastWeaponChangeTime < weaponChangeCooldown)
        {
            return; // No permitir cambio de arma si el cooldown no ha terminado
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedWeaponIndex = (selectedWeaponIndex + 1) % lastTwoWeapons.Count;
            SwitchWeapon(selectedWeaponIndex);
            lastWeaponChangeTime = Time.time; // Actualizar el tiempo del �ltimo cambio de arma
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

    // M�todo para recoger un arma nueva
    public void PickUpWeapon(GameObject newWeapon) // Recibe el GameObject del arma
    {
        weapons.Add(newWeapon); // A�adir el arma a la lista

        currentWeapon = newWeapon; // Establecer el arma actual
        Debug.Log("Picked up weapon: " + newWeapon.name);

        // Actualizar la lista de las dos �ltimas armas recogidas
        if (lastTwoWeapons.Count == 2)
        {
            lastTwoWeapons.RemoveAt(0); // Eliminar la arma m�s antigua
        }
        lastTwoWeapons.Add(newWeapon); // A�adir la nueva arma

        // Establecer el GameObject del arma en el Transform especificado
        if (weaponHolder != null && newWeapon != null) // Si el weaponHolder y el GameObject del arma no est�n vac�os
        {
            newWeapon.transform.SetParent(weaponHolder); // Establecer el padre del arma
            newWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Resetea la posici�n local y la rotaci�n local
            SetCurrentWeapon(newWeapon); // Establecer el arma actual
        }
        else
        {
            Debug.LogWarning("Weapon holder or weapon GameObject is not assigned.");
        }
    }

    // M�todo para cambiar de arma
    public void SwitchWeapon(int index)
    {
        if (index < lastTwoWeapons.Count) // Verificar que el �ndice sea v�lido para las dos �ltimas armas
        {
            SetCurrentWeapon(lastTwoWeapons[index]); // Establecer el arma actual seg�n el �ndice
            int weaponDataIndex = weapons.IndexOf(lastTwoWeapons[index]);
            if (weaponInfoDisplay != null && weaponDataArray[weaponDataIndex] != null)
            {
                weaponInfoDisplay.DisplayWeaponImage(weaponDataArray[weaponDataIndex].weaponIcon); // Actualizar el icono del arma
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
            Debug.LogWarning("WeaponsMenuHandler: �ndice fuera de los l�mites de los arrays.");
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
