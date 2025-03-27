using UnityEngine;

public class VelocityUpgrade : MonoBehaviour , IInteractable
{
    
    PlayerMovement playerMovement;

    [Header("Upgrades")]
    [SerializeField] float walkUpgrade;
    [SerializeField] float sprintUpgrade;

    private void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }
    public void OnInteract()
    {
        playerMovement.walkSpeed += walkUpgrade;
        playerMovement.sprintSpeed += sprintUpgrade;
        Destroy(gameObject);
    }

}
