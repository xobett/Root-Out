using UnityEngine;
using Weapons;

public class Rifle : WeaponsBase
{
    [Header("Balas a disparar")]
    [SerializeField] private int bullet = 1;

    protected override int NumBullets()
    {
        return bullet; // Número de perdigones por disparo
    }
}

