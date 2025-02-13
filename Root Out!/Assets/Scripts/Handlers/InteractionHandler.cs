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

    public bool Interact()
    {
        // Verifica si el usuario está intentando interactuar
        if (IsInteracting())
        {
            // Realiza un raycast desde la posición del objeto hacia adelante
            if (Physics.Raycast(transform.position, transform.forward * range, out outHit, range, whatIsInteraction))
            {
                // Llama al método OnInteract del objeto con el que se ha colisionado
                // outHit.collider.GetComponent<IInteractable>().OnInteract();
                Debug.Log("Tomado");
                return true; // Indica que la interacción fue exitosa
            }
        }
        return false; // Indica que no hubo interacción
    }

    public bool IsInteracting()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * range);
    }
}
