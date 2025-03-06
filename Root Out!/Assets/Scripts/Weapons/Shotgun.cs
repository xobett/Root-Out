using UnityEngine;
using Weapons;
public class Shotgun : WeaponsBase, IInteractable
{
    [SerializeField] int pelletsPerShot = 6; // Número de perdigones por disparo
    [SerializeField] private Transform weaponLocation; // Referencia al transform del arma

    public void OnInteract()
    {
        transform.SetParent(weaponLocation); // Asigna el transform del arma como padre
    }

    // Sobrescribir el método para definir el número de balas a disparar
    protected override int GetNumBullets()
    {
        return pelletsPerShot; // Número de perdigones por disparo
    }
}

