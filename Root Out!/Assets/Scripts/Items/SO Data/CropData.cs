
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

    [SerializeField] private float cooldownTime;
    [SerializeField] private float damage;

    [Header("VISUAL ICON")]

    [SerializeField] private Sprite cropIcon;

    [Header("PREFAB")]
    [SerializeField] private GameObject cropPrefab;


    public string CropName => cropName;
    public string CropDescription => cropDescription;
    public GameObject CropPrefab => cropPrefab;
}

public enum CropType
{
    None, Attack, Defense
}
