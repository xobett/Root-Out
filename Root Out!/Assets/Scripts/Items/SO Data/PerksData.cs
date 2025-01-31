using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Data/Create new Perk Data")]
public class PerksData : ScriptableObject
{
    [Header("GENERAL INFORMATION DATA")]
    [SerializeField] private string perkName;
    [SerializeField] private string perkDescription;

    [Header("VISUAL ICON")]
    [SerializeField] private Sprite perkIcon;
}
