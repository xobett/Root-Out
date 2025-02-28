using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [Header("INTERACTION SETTINGS")]
    [SerializeField] private float range;

    [SerializeField] private Transform rayPoint;

    [SerializeField] private LayerMask whatIsInteraction;

    private RaycastHit outHit;

    void Update()
    {
        Interact();
    }

    public void Interact()
    {
        // Verifica si el usuario está intentando interactuar
        if (IsInteracting())
        {
            // Realiza un raycast desde la posición del objeto hacia adelante
            if (Physics.Raycast(rayPoint.position, transform.forward * range, out outHit, range, whatIsInteraction))
            {
                Debug.Log("Interacting with: " + outHit.collider.name);
                // Llama al método OnInteract del objeto con el que se ha colisionado
                outHit.collider.GetComponent<IInteractable>().OnInteract();
            }
        }
    }

    public bool IsInteracting()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayPoint.position, transform.forward * range);
    }
}
