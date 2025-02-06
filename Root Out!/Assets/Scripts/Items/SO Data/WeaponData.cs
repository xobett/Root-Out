using UnityEngine;
using Weapons;

[CreateAssetMenu(fileName = "New weapon", menuName = "Data/Create new Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("GENERAL INFORMATION SETTINGS")]
    [SerializeField] private string weaponName;
    [SerializeField] private string weaponDescription;
    [SerializeField] private WeaponsBase.WeaponType weaponType;

    [Header("STATISTICS SETTINGS")]
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;

    [Header("AMMO SETTINGS")]
    [SerializeField] private float reloadTime;
    [SerializeField] private int maxAmmo;

    [Header("VISUAL ICON")]
    [SerializeField] private Sprite weaponIcon;

    [Header("PREFAB")]
    [SerializeField] private GameObject weaponPrefab;

}
