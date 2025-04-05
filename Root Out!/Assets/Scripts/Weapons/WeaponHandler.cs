
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

public class WeaponHandler : MonoBehaviour
{

    [SerializeField] public GameObject currentWeapon; // Arma actual
    [SerializeField] public Transform weaponHolder; // Transform donde se instanciará el arma

    [SerializeField] public List<GameObject> weapons = new List<GameObject>();  // Lista para almacenar las armas
    [SerializeField] private List<Image> weaponIcons = new List<Image>(); // Iconos en la rueda

    // Canvas de la rueda de armas
    [SerializeField] private RectTransform weaponSelectionWheel;

    // Índice de la selección actual en la rueda de armas
    private int selectedWeaponIndex = 0;

    // Variable para rastrear el tiempo del último cambio de arma
    private float lastWeaponChangeTime = 0f;
    // Cooldown de 0.5 segundos
    private const float weaponChangeCooldown = 0.7f;

    // Velocidad de rotación de la rueda de armas
    [SerializeField] float rotationSpeed = 5f;

    private void Awake()
    {
        weaponSelectionWheel.gameObject.SetActive(false); // Desactivar la rueda de armas al inicio
    }
    private void Update()
    {
        HandleMouseScroll();  // Maneja la rotación de la rueda del ratón para cambiar de arma
    }


    // Maneja la rotación de la rueda del ratón para cambiar de arma
    private void HandleMouseScroll()
    {
        if (currentWeapon != null && !currentWeapon.GetComponent<WeaponsBase>().isReloading)
        {

            if (weapons.Count < 2)
            {
                return; // No permitir rotación si hay menos de dos armas
            }

            if (Time.time - lastWeaponChangeTime < weaponChangeCooldown)
            {
                return; // No permitir cambio de arma si el cooldown no ha terminado
            }

            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput > 0f) // Si el ratón se desplaza hacia arriba
            {
                for (int i = selectedWeaponIndex + 1; i < weapons.Count; i++)
                {
                    if (weapons[i] != null)
                    {
                        StartCoroutine(RotateWeaponSelectionWheel(60f)); // Rotar 60 grados hacia arriba
                        selectedWeaponIndex = i; // Cambiar el índice de selección
                        SwitchWeapon(selectedWeaponIndex); // Cambiar el arma
                        lastWeaponChangeTime = Time.time; // Actualizar el tiempo del último cambio de arma
                        StartCoroutine(ColdDownWheelAnimation());

                        var playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
                        playerMovement.SetAnimationState(GetCurrentAim());
                        break;
                    }
                }
            }
            else if (scrollInput < 0f) // Si el ratón se desplaza hacia abajo
            {
                for (int i = selectedWeaponIndex - 1; i >= 0; i--)
                {
                    if (weapons[i] != null)
                    {
                        StartCoroutine(RotateWeaponSelectionWheel(-60f)); // Rotar 60 grados hacia abajo
                        selectedWeaponIndex = i;
                        SwitchWeapon(selectedWeaponIndex);
                        lastWeaponChangeTime = Time.time; // Actualizar el tiempo del último cambio de arma
                        StartCoroutine(ColdDownWheelAnimation());

                        var playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
                        playerMovement.SetAnimationState(GetCurrentAim());
                        break;
                    }
                }
            } 
        }
    }

    private IEnumerator ColdDownWheelAnimation()
    {
        weaponSelectionWheel.gameObject.SetActive(true); // Asegurarse de que la rueda esté activa
        Animation anim = weaponSelectionWheel.GetComponent<Animation>();
        anim.Play("RuedaArmasAbajo");
        yield return new WaitForSeconds(anim["RuedaArmasAbajo"].length);
        anim.Play("RuedaArmasArriba");
        yield return new WaitForSeconds(anim["RuedaArmasArriba"].length);
        weaponSelectionWheel.gameObject.SetActive(false); // Asegurarse de que la rueda esté activa
    }

    private IEnumerator RotateWeaponSelectionWheel(float targetValue) // Método para rotar la rueda de selección de armas
    {

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


        yield return null;
    } 

    // Método para recoger un arma nueva
    public void PickUpWeapon(GameObject newWeapon, WeaponData newWeaponData)
    {
        if (weapons.Count < 6) // Limitar el número de armas a 6
        {
            weapons.Insert(0, newWeapon); // Insertar el arma en la primera posición

            if (weapons.Count > 6)
            {
                weapons.RemoveAt(6); // Mantener un máximo de 6 armas
            }

            Debug.Log("Picked up weapon: " + newWeapon.name);

            // Desactivar el collider del arma para que no se pueda volver a recoger
            if (newWeapon.TryGetComponent<Collider>(out var weaponCollider))
            {
                weaponCollider.enabled = false;
            }

            // Ubicar el arma en el WeaponHolder
            if (weaponHolder != null && newWeapon != null)
            {
                newWeapon.transform.SetParent(weaponHolder);
                newWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Weapon holder or weapon GameObject is not assigned.");
            }

            // Asignar el WeaponData al nuevo componente
            var weaponComponent = newWeapon.GetComponent<WeaponComponent>();
            if (weaponComponent == null)
            {
                weaponComponent = newWeapon.AddComponent<WeaponComponent>();
            }
            weaponComponent.weaponData = newWeaponData;

            // Desactivar todas las armas antes de activar la nueva
            foreach (var weapon in weapons)
            {
                if (weapon != null && weapon != newWeapon)
                {
                    weapon.SetActive(false);
                }
            }

            // Activar la nueva arma
            newWeapon.SetActive(true);

            // **Actualizar la lista de iconos en el mismo orden**
            UpdateWeaponIcons();

            // **Actualizar posiciones de los íconos en el UI**
            UpdateWeaponPositions();

            // Establecer la nueva arma como el arma actual
            SetCurrentWeapon(newWeapon);

            // Activar la rueda de selección de armas si no está activa
            if (!weaponSelectionWheel.gameObject.activeSelf)
            {
                weaponSelectionWheel.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Cannot pick up more than 6 weapons.");
        }
    }

    // Método para sincronizar los iconos con las armas en el mismo orden
    private void UpdateWeaponIcons()
    {
        for (int i = 0; i < weaponIcons.Count; i++)
        {
            if (i < weapons.Count && weapons[i] != null) // Verifica que haya un arma en la posición
            {
                WeaponComponent weaponComponent = weapons[i].GetComponent<WeaponComponent>(); // Obtener el componente WeaponComponent

                if (weaponComponent != null && weaponComponent.weaponData != null)
                {
                    weaponIcons[i].sprite = weaponComponent.weaponData.weaponIcon; // Asignar el ícono correcto
                    weaponIcons[i].enabled = true; // Habilitar la imagen
                }
                else
                {
                    weaponIcons[i].enabled = false; // Deshabilitar si no hay arma
                }
            }
            else
            {
                weaponIcons[i].enabled = false; // Si no hay arma en la posición, desactivar ícono
            }
        }
    }

    // Método para posicionar correctamente los íconos en el UI
    private void UpdateWeaponPositions()
    {
        for (int i = 0; i < weaponIcons.Count; i++)
        {
            if (i < weapons.Count)
            {
                weaponIcons[i].transform.SetParent(weaponSelectionWheel);
                weaponIcons[i].transform.localPosition = weaponIcons[i].transform.localPosition; // Ajustar posición si es necesario
            }
        }
    }

    // Método para cambiar de arma
    public void SwitchWeapon(int index)
    {
        if (index < weapons.Count) // Verificar que el índice sea válido para todas las armas
        {
            SetCurrentWeapon(weapons[index]); // Establecer el arma actual según el índice\
        }
        else
        {
            Debug.LogWarning("No weapon in slot " + (index + 1)); // Mostrar advertencia si no hay un arma en el índice
        }
    }

    private PlayerAimState GetCurrentAim()
    {
        var aim = currentWeapon.GetComponentInChildren<AimType>().tipoDeApuntado;
        return aim;
    }

    // Método para establecer el arma actual y desactivar las otras armas
    private void SetCurrentWeapon(GameObject newWeapon)
    {
        // Desactivar todas las armas antes de activar la nueva
        foreach (var weapon in weapons)
        {
            if (weapon != null && weapon != newWeapon)
            {
                weapon.SetActive(false);
            }
        }

        currentWeapon = newWeapon; // Establecer la nueva arma
        currentWeapon.SetActive(true); // Activar la nueva arma
    }

    //// Seleccionar un arma según el índice
    //void SelectWeapon(int index) // Recibe el índice del arma
    //{
    //    if (IsValidIndex(index)) // Verificar si el índice es válido
    //    {
    //        SwitchWeapon(index); // Cambiar al arma seleccionada
    //    }
    //}

    //// Verificar si el índice es válido
    //private bool IsValidIndex(int index) // Recibe el índice del arma
    //{
    //    return index >= 0 && index < weapons.Count; // Verificar si el índice está dentro de los límites de los arrays
    //}

    // Dibujar gizmos en el editor para visualizar el weaponHolder
    private void OnDrawGizmos()
    {
        if (weaponHolder != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(weaponHolder.position, weaponHolder.forward * 10);
        }
    }
}

public class WeaponComponent : MonoBehaviour
{
    public WeaponData weaponData;
}
