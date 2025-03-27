using UnityEngine;

public class FinalEventActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isNear;

    public void OnInteract()
    {
        if (isNear)
        {
            GameManager.instance.StartFinaEvent();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isNear = false;
    }
}
