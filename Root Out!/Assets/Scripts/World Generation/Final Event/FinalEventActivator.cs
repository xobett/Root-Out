using UnityEngine;

public class FinalEventActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isNear;

    public void OnInteract()
    {
        if (isNear)
        {
            Debug.Log("Final Event should start");
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
