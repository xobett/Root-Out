using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponHandler : MonoBehaviour
{
    // Arma actual
    [SerializeField] public GameObject currentWeapon;
    // Transform donde se instanciar� el arma
    [SerializeField] public Transform weaponHolder;

    // Lista para almacenar las armas
    [SerializeField] public List<GameObject> weapons = new List<GameObject>();
    // Iconos en la rueda
    [SerializeField] private List<Image> weaponIcons = new List<Image>();

    // Canvas de la rueda de armas
    [SerializeField] private RectTransform weaponSelectionWheel;

    // �ndice de la selecci�n actual en la rueda de armas
    private int selectedWeaponIndex = 0;

    // Variable para rastrear el tiempo del �ltimo cambio de arma
    private float lastWeaponChangeTime = 0f;
    // Cooldown de 0.5 segundos
    private const float weaponChangeCooldown = 0.2f;

    // Velocidad de rotaci�n de la rueda de armas
    [SerializeField] float rotationSpeed = 100f;
   // private bool wheelIsRotating = false;

    private void Start()
    {
        weaponSelectionWheel.gameObject.SetActive(false);
    }
    private void Update()
    {
        // Maneja la rotaci�n de la rueda del rat�n para cambiar de arma
        HandleMouseScroll();
        // Maneja la selecci�n de arma al hacer clic
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

    // Maneja la rotaci�n de la rueda del rat�n para cambiar de arma
    private void HandleMouseScroll()
    {
        if (weapons.Count <= 1)
        {
            return; // No permitir rotaci�n si solo hay una arma
        }

        if (Time.time - lastWeaponChangeTime < weaponChangeCooldown)
        {
            return; // No permitir cambio de arma si el cooldown no ha terminado
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput > 0f) // Si el rat�n se desplaza hacia arriba
        {
            if (selectedWeaponIndex < weapons.Count - 1) // Verificar si no se ha alcanzado el l�mite superior
            {
                StartCoroutine(RotateWeaponSelectionWheel(60f)); // Rotar 60 grados hacia arriba
                selectedWeaponIndex = (selectedWeaponIndex + 1) % weapons.Count;
                SwitchWeapon(selectedWeaponIndex);
                lastWeaponChangeTime = Time.time; // Actualizar el tiempo del �ltimo cambio de arma
            }
        }
        else if (scrollInput < 0f) // Si el rat�n se desplaza hacia abajo
        {
            if (selectedWeaponIndex > 0) // Verificar si no se ha alcanzado el l�mite inferior
            {
                StartCoroutine(RotateWeaponSelectionWheel(-60f)); // Rotar 60 grados hacia abajo
                selectedWeaponIndex = (selectedWeaponIndex - 1 + weapons.Count) % weapons.Count;
                SwitchWeapon(selectedWeaponIndex);
                lastWeaponChangeTime = Time.time; // Actualizar el tiempo del �ltimo cambio de arma
            }
        }
    }

    private IEnumerator RotateWeaponSelectionWheel(float targetValue)
    {
       // wheelIsRotating = true; // Marcar que la rueda est� rotando
        var selectionWheelRect = weaponSelectionWheel.GetComponent<RectTransform>(); // Obtener el RectTransform de la rueda

        Quaternion targetRotation = Quaternion.Euler(0, 0, weaponSelectionWheel.transform.eulerAngles.z + targetValue); // Calcular la rotaci�n objetivo

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
        if (weapons.Count < 6) // Limitar el n�mero de armas a 6
        {
            weapons.Add(newWeapon); // A�adir el arma a la lista

            currentWeapon = newWeapon; // Establecer el arma actual
            Debug.Log("Picked up weapon: " + newWeapon.name);

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

            // Actualizar los iconos de las 
            UpdateWeaponPositions();

            // Asignar el icono del arma al slot correspondiente
            int weaponIndex = weapons.Count - 1; // Obtener el �ndice del arma reci�n a�adida
            if (weaponIndex < weaponIcons.Count)
            {
                weaponIcons[weaponIndex].sprite = newWeaponData.weaponIcon; // Asignar el icono del arma
                weaponIcons[weaponIndex].enabled = true; // Asegurarse de que el icono est� habilitado
                weaponIcons[weaponIndex].transform.localPosition = weaponIcons[weaponIndex].transform.localPosition; // Mantener la posici�n local del icono
            }
            else
            {
                // Si no hay suficientes slots de iconos, crear uno nuevo
                Image newIcon = Instantiate(weaponIcons[0], weaponSelectionWheel); // Instanciar un nuevo icono basado en el primer icono
                newIcon.sprite = newWeaponData.weaponIcon; // Asignar el icono del arma
                newIcon.enabled = true; // Asegurarse de que el icono est� habilitado
                weaponIcons.Add(newIcon); // A�adir el nuevo icono a la lista
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
                // Asignar la posici�n del icono en el canvas
                weaponIcons[i].transform.SetParent(weaponSelectionWheel);
                weaponIcons[i].transform.localPosition = weaponIcons[i].transform.localPosition;
            }
        }
    }

    // M�todo para cambiar de arma
    public void SwitchWeapon(int index)
    {
        if (index < weapons.Count) // Verificar que el �ndice sea v�lido para todas las armas
        {
            SetCurrentWeapon(weapons[index]); // Establecer el arma actual seg�n el �ndice
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
    void SelectWeapon(int index) // Recibe el �ndice del arma
    {
        if (IsValidIndex(index)) // Verificar si el �ndice es v�lido
        {
            SwitchWeapon(index); // Cambiar al arma seleccionada
        }
    }

    // Verificar si el �ndice es v�lido
    private bool IsValidIndex(int index) // Recibe el �ndice del arma
    {
        return index >= 0 && index < weapons.Count; // Verificar si el �ndice est� dentro de los l�mites de los arrays
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
