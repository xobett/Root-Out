using UnityEngine;
using Weapons;
public class Shotgun : WeaponsBase, IInteractable
{
    [SerializeField] int pelletsPerShot = 6; // N�mero de perdigones por disparo
    [SerializeField] private Transform weaponLocation; // Referencia al transform del arma

    public void OnInteract()
    {
        transform.SetParent(weaponLocation); // Asigna el transform del arma como padre
    }

    // Sobrescribir el m�todo para definir el n�mero de balas a disparar
    protected override int GetNumBullets()
    {
        return pelletsPerShot; // N�mero de perdigones por disparo
    }
}

