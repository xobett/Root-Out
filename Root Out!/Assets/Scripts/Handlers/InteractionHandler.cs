using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [Header("INTERACTION SETTINGS")]
    [SerializeField] private float range;

    [SerializeField] private LayerMask whatIsInteraction;

    private RaycastHit outHit;

    void Update()
    {
        Interact();
    }

    private void Interact()
    {
        if (IsInteracting())
        {
            if (Physics.Raycast(transform.position, transform.forward * range, out outHit, range, whatIsInteraction))
            {
                outHit.collider.GetComponent<IInteractable>().OnInteract();
            }
        }
    }

    private bool IsInteracting()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * range);
    }
}
