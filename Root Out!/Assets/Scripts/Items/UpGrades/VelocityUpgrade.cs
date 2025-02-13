using UnityEngine;

public class VelocityUpgrade : MonoBehaviour
{
    [Header("Ref Scripts")]
    [SerializeField] InteractionHandler interactionHandler;
    [SerializeField] PlayerMovement playerMovement;

    [Header("Upgrades")]
    [SerializeField] float walkUpgrade;
    [SerializeField] float sprintUpgrade;

    private void Update()
    {
        VelUpgrade();
    }
    private void VelUpgrade()
    {
        if (interactionHandler.Interact())
        {
            playerMovement.walkSpeed = walkUpgrade;
            playerMovement.sprintSpeed = sprintUpgrade;
            Destroy(gameObject);    
        }
    }
}
