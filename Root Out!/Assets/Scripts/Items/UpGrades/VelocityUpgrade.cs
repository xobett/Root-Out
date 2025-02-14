using UnityEngine;

public class VelocityUpgrade : MonoBehaviour , IInteractable
{
    [Header("Ref Scripts")]
    [SerializeField] PlayerMovement playerMovement;

    [Header("Upgrades")]
    [SerializeField] float walkUpgrade;
    [SerializeField] float sprintUpgrade;

    public void OnInteract()
    {
        playerMovement.walkSpeed = walkUpgrade;
        playerMovement.sprintSpeed = sprintUpgrade;
        Destroy(gameObject);
    }

}
