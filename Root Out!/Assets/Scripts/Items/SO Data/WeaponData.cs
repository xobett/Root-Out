using UnityEngine;
using Weapons;

[CreateAssetMenu(fileName = "New weapon", menuName = "Data/Create new Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("GENERAL INFORMATION SETTINGS")]
    [SerializeField] public string weaponName;
    [SerializeField] public string weaponDescription;
    [SerializeField] public WeaponsBase.WeaponType weaponType;

    [Header("STATISTICS SETTINGS")]
    [SerializeField] public int damage;
    [SerializeField] public float range;
    [SerializeField] public float fireRate;

    [Header("AMMO SETTINGS")]
    [SerializeField] public float reloadTime;
    [SerializeField] public int maxAmmo;

    [Header("VISUAL ICON")]
    [SerializeField] public Sprite weaponIcon;

    [Header("PREFAB")]
    [SerializeField] public GameObject weaponPrefab;

}
