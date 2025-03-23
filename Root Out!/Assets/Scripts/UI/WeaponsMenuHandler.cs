using UnityEngine;
using UnityEngine.UI;

public class WeaponsMenuHandler : MonoBehaviour
{

    public Image[] weaponIcons; // Iconos en la rueda
    public string[] weaponNames; // Nombres de las armas (para debug o visualización)
    public WeaponData[] weaponDataArray; // Datos (por ejemplo, iconos ampliados) de las armas

    // Referencia al WeaponHandler para comunicar el cambio de arma
    public WeaponHandler weaponHandler;

    // Índice de la selección actual en la rueda de armas
    private int selectedWeaponIndex = 0;

    void Update()
    {
        if (weaponIcons.Length == 0)
        {
            Debug.LogWarning("WeaponsMenuHandler: No hay iconos de armas asignados.");
            return;
        }

        // Rotar entre las secciones usando el scroll del mouse
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedWeaponIndex = (selectedWeaponIndex + 1) % weaponIcons.Length;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedWeaponIndex = (selectedWeaponIndex - 1 + weaponIcons.Length) % weaponIcons.Length;
        }

        // Actualiza visualmente la selección (por ejemplo, resaltando en amarillo al arma seleccionada)
        for (int i = 0; i < weaponIcons.Length; i++)
        {
            weaponIcons[i].color = (i == selectedWeaponIndex) ? Color.yellow : Color.white;
        }

        // Al hacer clic, se confirma la selección del arma
        if (Input.GetMouseButtonDown(0))
        {
            SelectWeapon(selectedWeaponIndex);
        }
    }

    /// <summary>
    /// Método que se llama al confirmar la selección del arma,
    /// y se encarga de mostrar información (por ejemplo, actualizando la imagen en pantalla)
    /// y de notificar al WeaponHandler para cambiar el arma.
    /// </summary>
    /// index
    /// Índice del arma seleccionado (corresponde al ícono actual)
    void SelectWeapon(int index)
    {
        // Validación básica de índices en los arrays de datos (opcional)
        if (index >= 0 && index < weaponNames.Length && index < weaponDataArray.Length)
        {
            Debug.Log("WeaponsMenuHandler: Seleccionaste: " + weaponNames[index]);

            // Notificar a WeaponHandler para cambiar el arma según el índice seleccionado
            if (weaponHandler != null)
            {
                weaponHandler.SwitchWeapon(index);
            }
            else
            {
                Debug.LogWarning("WeaponsMenuHandler: WeaponHandler no está asignado.");
            }
        }
        else
        {
            Debug.LogWarning("WeaponsMenuHandler: Índice fuera de los límites de los arrays.");
        }
    }
}