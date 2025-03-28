using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public void ItemInteraction()
    {
        gameObject.GetComponent<IInteractable>().OnInteract();
    }
}
