using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponPrefabs = new GameObject[3]; // Arreglo para almacenar los prefabs de las armas
    [SerializeField] public GameObject currentWeapon; // Arma actual
    [SerializeField] public Transform weaponHolder; // Transform donde se instanciar� el arma
    private int weaponCount = 0; // Contador de armas recogidas

    private void Update()
    {
        WeaponChance(); // Llama al m�todo para cambiar de arma
    }

    void WeaponChance()
    {
        // Cambiar de arma usando las teclas 1, 2 y 3
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

    // M�todo para recoger un arma nueva
    public void PickUpWeapon(GameObject newWeaponPrefab) // Recibe el prefab del arma
    {
        if (weaponCount < 3)
        {
            // A�adir el arma al arreglo si no se ha alcanzado el m�ximo
            weaponPrefabs[weaponCount] = newWeaponPrefab; // A�adir el arma al arreglo
            weaponCount++; // Incrementar el contador de armas
        }
        else
        {
            // Si ya se tiene el m�ximo de armas, reemplazar la m�s antigua
            for (int i = 1; i < 3; i++) // Recorrer el arreglo de armas
            {
                weaponPrefabs[i - 1] = weaponPrefabs[i]; // Reemplazar el arma anterior
            }
            weaponPrefabs[3 - 1] = newWeaponPrefab; // Reemplazar la �ltima arma
        }

        currentWeapon = newWeaponPrefab; // Establecer el arma actual
        Debug.Log("Picked up weapon: " + newWeaponPrefab.name);

        // Instanciar el prefab del arma en el Transform especificado
        if (weaponHolder != null && newWeaponPrefab != null) // Si el weaponHolder y el prefab del arma no estan vacios
        {
            newWeaponPrefab.transform.SetParent(weaponHolder); // Establecer el padre del arma
            newWeaponPrefab.transform.SetLocalPositionAndRotation(weaponHolder.localPosition,weaponHolder.localRotation);
            SetCurrentWeapon(newWeaponPrefab); // Establecer el arma actual
        }
        else
        {
            Debug.LogWarning("Weapon holder or weapon prefab is not assigned.");
        }
    }

    // M�todo para cambiar de arma
    private void SwitchWeapon(int index)
    {
        if (index < weaponCount) // Si el �ndice es menor que el n�mero de armas recogidas
        {
            SetCurrentWeapon(weaponPrefabs[index]); // Establecer el arma actual seg�n el �ndice
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
        foreach (var weapon in weaponPrefabs)
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
        Gizmos.color = Color.green;
        Gizmos.DrawRay(weaponHolder.position, weaponHolder.forward * 100);
    }
}
