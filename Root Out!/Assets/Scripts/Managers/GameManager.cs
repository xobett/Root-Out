using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public enum GrowthSelection
{
    Compelling, Genuine, Marvelous
}

public class GameManager : MonoBehaviour
{
    //Enum creado para clasificar tipo de crecimiento de girasol.

    //Gamem Manager estatico para acceder a el desde cualquier script.
    public static GameManager instance;

    [Header("UI PANEL SETTINGS")]
    //Panel de UI de Seleccion de Crecimiento.
    [SerializeField] private GameObject uiGrowthSelectionPanel;
    //Panel de UI de fondo negro.
    [SerializeField] private GameObject uiDarkPanel;

    //Inputs referenciados para desactivar en un evento activo u activar al final de uno.
    private PlayerMovement playerMovement;
    private LeafJump leafJump;
    private CameraFollow cameraFollow;

    //Animators usados para activar y desactivar condiciionales.
    private Animator growthSelectionAnimator;
    private Animator darkPanelAnimator;

    //Enum usado para decidir metodo de generacion de mundo.
    private GrowthSelection growthSelection;

    //Bool usado para cerrar un evento activo.
    private bool activeEvent;

    //Sunflower referenciado actualmente para crecer
    [SerializeField] private Sunflower sunflowerToGrow;
    
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

        GetInputReferences();
        GetAnimatorReferences();
    }

    private void Update()
    {
        if (activeEvent && IsEscaping())
        {
            CloseActiveEvent();
        }
    }

    private void CloseActiveEvent()
    {
        growthSelectionAnimator.SetBool("isActive", false);
        darkPanelAnimator.SetBool("isActive", false);

        growthSelectionAnimator.SetTrigger("Default");
        darkPanelAnimator.SetTrigger("Default");

        RegainInput();

        activeEvent = false;
    }

    public void GrowthSelectionEvent(Sunflower sunflower)
    {
        sunflowerToGrow = sunflower;

        DeactivateInput();
        activeEvent = true;

        var growthPanelAnimator = uiGrowthSelectionPanel.GetComponent<Animator>();
        var darkPanelAnimator = uiDarkPanel.GetComponent<Animator>();

        growthPanelAnimator.SetBool("isActive", true);
        darkPanelAnimator.SetBool("isActive", true);

    }

    public void SendType(string selectionType)
    {
        switch (selectionType)
        {
            case "Marvelous":
                {
                    growthSelection = GrowthSelection.Marvelous;
                    break;
                }

            case "Genuine":
                {
                    growthSelection = GrowthSelection.Genuine;
                    break;
                }

            case "Compelling":
                {
                    growthSelection = GrowthSelection.Compelling;
                    break;
                }
        }

        sunflowerToGrow.GrowSunflower(growthSelection);

        CloseActiveEvent();
    }

    //Deactivate buttons upon selection

    #region InputMethods
    public void DeactivateInput()
    {
        playerMovement.enabled = false;
        leafJump.enabled = false;
        cameraFollow.enabled = false;

        Cursor.lockState = CursorLockMode.None;
    }
    public void RegainInput()
    {
        playerMovement.enabled = true;
        leafJump.enabled = true;
        cameraFollow.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
    }
    private bool IsEscaping() => Input.GetKeyDown(KeyCode.Escape);
    #endregion

    #region Reference Methods
    private void GetInputReferences()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        leafJump = GameObject.FindGameObjectWithTag("Player").GetComponent<LeafJump>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }
    private void GetAnimatorReferences()
    {
        growthSelectionAnimator = uiGrowthSelectionPanel.GetComponent<Animator>();
        darkPanelAnimator = uiDarkPanel.GetComponent<Animator>();
    }
    #endregion
}
