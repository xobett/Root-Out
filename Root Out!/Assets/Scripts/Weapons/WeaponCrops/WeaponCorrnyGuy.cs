using UnityEngine;
using Weapons;
public class WeaponCoryGuy : WeaponsBase
{
    [SerializeField] private AudioSource shootSource;
    [SerializeField] private AudioClip shootClip;

    protected override void Shoot()
    {
        base.Shoot();

        shootSource.Play();
    }
}
