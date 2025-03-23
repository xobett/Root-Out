using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponHandler : MonoBehaviour
{
    // Arma actual
    [SerializeField] public GameObject currentWeapon;
    // Transform donde se instanciará el arma
    [SerializeField] public Transform weaponHolder;

    // Lista para almacenar las armas
    [SerializeField] public List<GameObject> weapons = new List<GameObject>();
    // Iconos en la rueda
    [SerializeField] private List<Image> weaponIcons = new List<Image>();

    // Canvas de la rueda de armas
    [SerializeField] private RectTransform weaponSelectionWheel;

    // Índice de la selección actual en la rueda de armas
    private int selectedWeaponIndex = 0;

    // Variable para rastrear el tiempo del último cambio de arma
    private float lastWeaponChangeTime = 0f;
    // Cooldown de 0.5 segundos
    private const float weaponChangeCooldown = 0.2f;

    // Velocidad de rotación de la rueda de armas
    [SerializeField] float rotationSpeed = 100f;
   // private bool wheelIsRotating = false;

    private void Start()
    {
        weaponSelectionWheel.gameObject.SetActive(false);
    }
    private void Update()
    {
        // Maneja la rotación de la rueda del ratón para cambiar de arma
        HandleMouseScroll();
        // Maneja la selección de arma al hacer clic
        HandleWeaponSelection();
        OpenMenu();
    }

    private void OpenMenu()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            weaponSelectionWheel.gameObject.SetActive(true);
        }
        else
        {
            weaponSelectionWheel.gameObject.SetActive(false);
        }
    }

    // Maneja la rotación de la rueda del ratón para cambiar de arma
    private void HandleMouseScroll()
    {
        if (weapons.Count <= 1)
        {
            return; // No permitir rotación si solo hay una arma
        }

        if (Time.time - lastWeaponChangeTime < weaponChangeCooldown)
        {
            return; // No permitir cambio de arma si el cooldown no ha terminado
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput > 0f) // Si el ratón se desplaza hacia arriba
        {
            if (selectedWeaponIndex < weapons.Count - 1) // Verificar si no se ha alcanzado el límite superior
            {
                StartCoroutine(RotateWeaponSelectionWheel(60f)); // Rotar 60 grados hacia arriba
                selectedWeaponIndex = (selectedWeaponIndex + 1) % weapons.Count;
                SwitchWeapon(selectedWeaponIndex);
                lastWeaponChangeTime = Time.time; // Actualizar el tiempo del último cambio de arma
            }
        }
        else if (scrollInput < 0f) // Si el ratón se desplaza hacia abajo
        {
            if (selectedWeaponIndex > 0) // Verificar si no se ha alcanzado el límite inferior
            {
                StartCoroutine(RotateWeaponSelectionWheel(-60f)); // Rotar 60 grados hacia abajo
                selectedWeaponIndex = (selectedWeaponIndex - 1 + weapons.Count) % weapons.Count;
                SwitchWeapon(selectedWeaponIndex);
                lastWeaponChangeTime = Time.time; // Actualizar el tiempo del último cambio de arma
            }
        }
    }

    private IEnumerator RotateWeaponSelectionWheel(float targetValue)
    {
       // wheelIsRotating = true; // Marcar que la rueda está rotando
        var selectionWheelRect = weaponSelectionWheel.GetComponent<RectTransform>(); // Obtener el RectTransform de la rueda

        Quaternion targetRotation = Quaternion.Euler(0, 0, weaponSelectionWheel.transform.eulerAngles.z + targetValue); // Calcular la rotación objetivo

        float time = 0f;

        while (time < 1)
        {
            selectionWheelRect.rotation = Quaternion.Slerp(selectionWheelRect.rotation, targetRotation, time);
            time += Time.deltaTime * rotationSpeed;
            yield return null;
        }
        selectionWheelRect.rotation = targetRotation;

       // wheelIsRotating = false;

        yield return null;
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

            // Actualizar los iconos de las 
            UpdateWeaponPositions();

            // Asignar el icono del arma al slot correspondiente
            int weaponIndex = weapons.Count - 1; // Obtener el índice del arma recién añadida
            if (weaponIndex < weaponIcons.Count)
            {
                weaponIcons[weaponIndex].sprite = newWeaponData.weaponIcon; // Asignar el icono del arma
                weaponIcons[weaponIndex].enabled = true; // Asegurarse de que el icono esté habilitado
                weaponIcons[weaponIndex].transform.localPosition = weaponIcons[weaponIndex].transform.localPosition; // Mantener la posición local del icono
            }
            else
            {
                // Si no hay suficientes slots de iconos, crear uno nuevo
                Image newIcon = Instantiate(weaponIcons[0], weaponSelectionWheel); // Instanciar un nuevo icono basado en el primer icono
                newIcon.sprite = newWeaponData.weaponIcon; // Asignar el icono del arma
                newIcon.enabled = true; // Asegurarse de que el icono esté habilitado
                weaponIcons.Add(newIcon); // Añadir el nuevo icono a la lista
            }
        }
        else
        {
            Debug.LogWarning("Cannot pick up more than 6 weapons.");
        }
    }

    private void UpdateWeaponPositions()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i < weaponIcons.Count)
            {
                // Asignar la posición del icono en el canvas
                weaponIcons[i].transform.SetParent(weaponSelectionWheel);
                weaponIcons[i].transform.localPosition = weaponIcons[i].transform.localPosition;
            }
        }
    }

    // Método para cambiar de arma
    public void SwitchWeapon(int index)
    {
        if (index < weapons.Count) // Verificar que el índice sea válido para todas las armas
        {
            SetCurrentWeapon(weapons[index]); // Establecer el arma actual según el índice
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
            SwitchWeapon(index); // Cambiar al arma seleccionada
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
