using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiInventoryIconInfo : MonoBehaviour
{
    [SerializeField] private InventoryItemData item;

    public void DisplayItemInfo()
    {
        UIManager infoDisplayer = UIManager.instance;
        infoDisplayer.UpdateSelectedItemInfo(item);
    }

    public void GetItem(InventoryItemData data)
    {
        item = data;
    }
}
