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
    [SerializeField] private TextMeshProUGUI generalMessage;

    [SerializeField] private float fadeSpeed;

    [Header("INVENTORY PANEL SETTINGS")]
    [SerializeField] private GameObject inventoryPanel;

    [Header("INVENTORY GRID SETTINGS")]
    [SerializeField] private GameObject cropInventoryGrid;
    [SerializeField] private GameObject weaponInventoryGrid;
    [SerializeField] private GameObject perkInventoryGrid;

    [Header("PLAYER INVENTORY SETTINGS")]
    [SerializeField] private InventoryHandler playerInventory;

    [SerializeField] private int itemsDisplayed;

    private bool isOpened;

    private float targetValue;

    void Start()
    {
        GetPlayerInventory();
        GetInputReferences();
    }

    void Update()
    {
        if (IsOpening())
        {
            OpenInventory();
        }
    }

    private void DisplayTotalCoins()
    {
        totalCoinsText.text = $"Total coins: {playerInventory.SeedCoins}";

        if (totalCoinsText.alpha >= 0.9f)
        {
            targetValue = 0;
        }
        else if (totalCoinsText.alpha <= 0.1f)
        {
            targetValue = 1;
        }

        totalCoinsText.alpha = Mathf.Lerp(totalCoinsText.alpha, targetValue, fadeSpeed * Time.deltaTime);
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
                var itemProt = new GameObject();
                itemProt.AddComponent<Image>();
                itemProt.GetComponent<Image>().sprite = playerInventory.Inventory[i].ItemIcon;

                switch (playerInventory.Inventory[i].ItemType)
                {
                    case ItemType.Crop:
                        {
                            Instantiate(itemProt, cropInventoryGrid.transform);
                            break;
                        }

                    case ItemType.Weapon:
                        {
                            Instantiate(itemProt, weaponInventoryGrid.transform);
                            break;
                        }

                    case ItemType.Perk:
                        {
                            Instantiate(itemProt, perkInventoryGrid.transform);
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
        playerMovement.enabled = false;
        leafJump.enabled = false;
        cameraFollow.enabled = false;
        cropHandler.enabled = false;

        hudCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    private void RegainInput()
    {
        playerMovement.enabled = true;
        leafJump.enabled = true;
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
