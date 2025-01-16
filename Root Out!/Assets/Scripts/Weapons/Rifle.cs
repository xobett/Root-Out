using UnityEngine;

public class Rifle : WeaponsBase
{
    [Header("Balas a disparar")]
    [SerializeField] private int bullet = 1;

    protected override int GetNumBullets()
    {
        return bullet; // Número de perdigones por disparo
    }
}

