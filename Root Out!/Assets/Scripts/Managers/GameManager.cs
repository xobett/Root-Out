using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Input scripts reference
    private PlayerMovement playerMovement;
    private LeafJump leafJump;
    private CameraFollow cameraFollow;

    [SerializeField] private GameObject growthSelectionPanel;
    [SerializeField] private GameObject uiDarkPanel;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OpenGrowthSelection()
    {
        DeactivateInput();

        var growthPanelAnimator = growthSelectionPanel.GetComponent<Animator>();
        var darkPanelAnimator = uiDarkPanel.GetComponent<Animator>();

        growthPanelAnimator.SetBool("openSpawnSelection", true);
        darkPanelAnimator.SetBool("selectionActive", true);

    }

    public void DeactivateInput()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        leafJump = GameObject.FindGameObjectWithTag("Player").GetComponent<LeafJump>();
        cameraFollow = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraFollow>();

        playerMovement.enabled = false;
        leafJump.enabled = false;
        cameraFollow.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void RegainInput()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        leafJump = GameObject.FindGameObjectWithTag("Player").GetComponent<LeafJump>();
        cameraFollow = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraFollow>();

        playerMovement.enabled = true;
        leafJump.enabled = true;
        cameraFollow.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }
}
