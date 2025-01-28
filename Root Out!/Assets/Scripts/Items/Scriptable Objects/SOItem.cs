using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Item", menuName = "SO Item")]
public class SOItem : ScriptableObject
{
    public string itemName;
    public string itemType;
    public string itemDescription;
}
