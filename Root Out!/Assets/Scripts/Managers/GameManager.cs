using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum GrowthSelection
{
    //Enum creado para clasificar tipo de crecimiento de girasol.
    Compelling, Genuine, Marvelous
}

public class GameManager : MonoBehaviour
{
    //Gamem Manager estatico para acceder a el desde cualquier script.
    public static GameManager instance;

    //Inputs referenciados para desactivar en un evento activo u activar al final de uno.
    private PlayerMovement playerMovement;
    private LeafJump leafJump;
    private CameraFollow cameraFollow;

    [Header("SUNFLOWER UNLOCK EVENT SETTINGS")]
    [SerializeField] private Sunflower sunflowerToGrow;

    [SerializeField] private TextMeshProUGUI countdownTimerText;
    private float countdownTime = 60;

    //Bool usado para cerrar un evento activo.
    private bool activeEvent;

    //Sunflower referenciado actualmente para crecer
    
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
    }

    private void Update()
    {
        countdownTime -= Time.deltaTime;

        countdownTimerText.text = string.Format("{0:00}", countdownTime);
    }

    public void GrowSunflowerEvent(GrowthSelection growthType, Sunflower sunflower)
    {

        //Grab active sunflower to unlock, depending on the growth option what should do, FIRST CREATE THE TIMER

        sunflowerToGrow = sunflower;

        switch (growthType)
        {
            case GrowthSelection.Marvelous:
                {
                    Debug.Log("Marvelous way used");

                    break;
                }

            case GrowthSelection.Genuine:
                {
                    Debug.Log("Genuine way used");

                    break;
                }

            case GrowthSelection.Compelling:
                {
                    Debug.Log("Compelling way used");

                    break;
                }
        }
    }

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
    #endregion
}
