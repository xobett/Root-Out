using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    //Input scripts reference
    private PlayerMovement playerMovement;
    private LeafJump leafJump;
    private CameraFollow cameraFollow;
    private CropHandler cropHandler;

    [Header("HUD SETTINGS")]
    [SerializeField] private GameObject hudCanvas;

    [SerializeField] private TextMeshProUGUI totalCoinsText;
    [SerializeField] private TextMeshProUGUI totalAmmoText;

    [SerializeField] private TextMeshProUGUI generalMessage;

    [SerializeField] private float fadeSpeed;

    [Header("INVENTORY PANEL SETTINGS")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject uiInventoryIcon;

    [Header("INVENTORY GRID SETTINGS")]
    [SerializeField] private GameObject cropInventoryGrid;
    [SerializeField] private GameObject weaponInventoryGrid;
    [SerializeField] private GameObject perkInventoryGrid;

    [Header("PLAYER INVENTORY SETTINGS")]
    [SerializeField] private InventoryHandler playerInventory;

    [SerializeField] private int itemsDisplayed;

    public bool isOpened = false;

    [SerializeField] private PauseMenu pauseMenu;

    void Start()
    {
        GetPlayerInventory();
        GetInputReferences();
    }

    void Update()
    {
        if (IsOpening() && !pauseMenu.isPaused)
        {
            OpenInventory();
        }

        DisplayCoinsAndAmmo();
    }

    private void DisplayCoinsAndAmmo()
    {
        totalCoinsText.text = $"{playerInventory.SeedCoins}";
        totalAmmoText.text = $"{playerInventory.Ammo}";
    }

    private void OpenInventory()
    {
        isOpened = !isOpened;
        inventoryPanel.SetActive(isOpened);

        if (isOpened)
        {
            DeactivateInput();

            for (int i = itemsDisplayed; i < playerInventory.Inventory.Count; i++)
            {                
                GameObject uiIcon = Instantiate(uiInventoryIcon);
                uiIcon.GetComponent<Image>().sprite = playerInventory.Inventory[i].ItemIcon;

                var uiIconInfo = uiIcon.GetComponent<UiInventoryIconInfo>();
                uiIconInfo.GetItem(playerInventory.Inventory[i]);

                switch (playerInventory.Inventory[i].ItemType)
                {
                    case ItemType.Crop:
                        {
                            uiIcon.transform.SetParent(cropInventoryGrid.transform, false);
                            break;
                        }

                    case ItemType.Weapon:
                        {
                            uiIcon.transform.SetParent(weaponInventoryGrid.transform, false);
                            break;
                        }

                    case ItemType.Perk:
                        {
                            uiIcon.transform.SetParent(perkInventoryGrid.transform, false);
                            break;
                        }
                }

                itemsDisplayed++;
                Debug.Log($"Inventory item number : {i}");
            }
            
        }
        else
        {
            RegainInput();
        }
    }

    private void DeactivateInput()
    {
        //playerMovement.enabled = false;
        //leafJump.enabled = false;
        GameManager.instance.gamePaused = true;
        cameraFollow.enabled = false;
        cropHandler.enabled = false;

        hudCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    private void RegainInput()
    {
        //playerMovement.enabled = true;
        //leafJump.enabled = true;
        GameManager.instance.gamePaused = false;
        cameraFollow.enabled = true;
        cropHandler.enabled = true;

        hudCanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    private void GetInputReferences()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        playerMovement = player.GetComponent<PlayerMovement>();
        leafJump = player.GetComponent<LeafJump>();
        cropHandler = player.GetComponent<CropHandler>();

        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    private void GetPlayerInventory()
    {
        playerInventory = GameManager.instance.playerInventoryHandler;
    }
    private bool IsOpening()
    {
        return Input.GetKeyDown(KeyCode.I);
    }

}
