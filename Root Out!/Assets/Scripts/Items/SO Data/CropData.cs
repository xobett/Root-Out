
using UnityEngine;


[CreateAssetMenu(fileName = "New Crop", menuName = "Data/Create new Crop Data")]
public class CropData : ScriptableObject
{
    [Header("GENERAL INFORMATION SETTINGS")]
    [SerializeField] private string cropName;
    [SerializeField] private string cropDescription;
    [SerializeField] private CropType type = CropType.None;

    [Header("HEALTH SETTINGS")]
    [SerializeField] private float cropHealth;

    [Header("MOVEMENT SETTINGS")]
    [SerializeField] private float cropWalkSpeed;
    [SerializeField] private float cropRunSpeed;

    [Header("ABILITY SETTINGS")]
    [SerializeField] private float cropCooldownTime;
    [SerializeField] private float cropDamage;

    [SerializeField] public int cooldownBypass;
    [SerializeField] public bool cooldownAnimation;

    [SerializeField] public float timer;

    [Header("VISUAL ICON")]

    [SerializeField] private Sprite cropIcon;

    [Header("PREFAB")]
    [SerializeField] private GameObject cropPrefab;


    public string CropName => cropName;
    public string CropDescription => cropDescription;
    public GameObject CropPrefab => cropPrefab;
    public CropType Type => type;
    public float CropCooldownTime => cropCooldownTime;
    public float CropDamage => cropDamage;
    public Sprite CropIcon => cropIcon;
}

public enum CropType
{
    None, Attack, Defense
}
