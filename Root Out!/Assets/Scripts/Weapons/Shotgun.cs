using UnityEngine;
using Weapons;

public class Shotgun : WeaponsBase
{
    [SerializeField] int pelletsPerShot = 6; // N�mero de perdigones por disparo


    // Sobrescribir el m�todo para definir el n�mero de balas a disparar
    protected override int NumBullets()
    {
        return pelletsPerShot; // N�mero de perdigones por disparo
    }
}

