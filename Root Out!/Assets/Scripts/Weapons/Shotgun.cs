using UnityEngine;

public class Shotgun : WeaponsBase
{
    [SerializeField] int pelletsPerShot = 6; // Número de perdigones por disparo


    // Sobrescribir el método para definir el número de balas a disparar
    protected override int GetNumBullets() 
    {
        return pelletsPerShot; // Número de perdigones por disparo
    }
}

