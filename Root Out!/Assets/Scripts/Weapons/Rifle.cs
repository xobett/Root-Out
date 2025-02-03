using UnityEngine;
using Weapons;

public class Rifle : WeaponsBase
{
   
    protected override void Shoot()
    {
        base.Shoot();
        CameraShake.StartShake(0.1f, 0.1f);
    }

}

