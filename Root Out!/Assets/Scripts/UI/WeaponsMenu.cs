using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsMenu : MonoBehaviour
{
    public Image[] weaponIcons; // Los iconos en la rueda
    public string[] weaponNames; // Los nombres de las armas
    public WeaponData[] weaponDataArray; // Array de Scriptable Objects de armas
    public WeaponInfoDisplay weaponInfoDisplay; // Referencia al script de visualizaci�n de la imagen del arma
    private int selectedWeaponIndex = 0;

    void Update()
    {
        if (weaponIcons.Length == 0)
        {
            Debug.LogWarning("No hay iconos de armas asignados.");
            return;
        }

        // Rotar entre las secciones (usa el eje vertical del mouse o el joystick)
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedWeaponIndex = (selectedWeaponIndex + 1) % weaponIcons.Length;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedWeaponIndex = (selectedWeaponIndex - 1 + weaponIcons.Length) % weaponIcons.Length;
        }

        // Actualizar visualmente la selecci�n
        for (int i = 0; i < weaponIcons.Length; i++)
        {
            weaponIcons[i].color = i == selectedWeaponIndex ? Color.yellow : Color.white;
        }

        // Seleccionar el arma cuando se suelta un bot�n
        if (Input.GetMouseButtonDown(0))
        {
            SelectWeapon(selectedWeaponIndex);
        }
    }

    void SelectWeapon(int index)
    {
        if (index >= 0 && index < weaponNames.Length && index < weaponDataArray.Length)
        {
            Debug.Log("Seleccionaste: " + weaponNames[index]);
            weaponInfoDisplay.DisplayWeaponImage(weaponDataArray[index].weaponIcon);
            // cambiar el arma activa en tu juego
        }
        else
        {
            Debug.LogWarning("�ndice fuera de los l�mites de los arrays.");
        }
    }
}
