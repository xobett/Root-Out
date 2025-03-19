using System.Collections;
using System.Linq.Expressions;
using TMPro;
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
    [SerializeField] private Sunflower currentSunflower;

    public Sunflower activeSunflower => currentSunflower;

    [SerializeField] private Animator currentSunflowerAnimator; 
    [SerializeField] private Animator currentSunflowerLifebarAnimator;

    [SerializeField] private TextMeshProUGUI timerText;
    private float countdownTimer;
    private float timeToCountdown = 10f;

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
        EventTimer();
    }

    private void EventTimer()
    {
        if (activeEvent)
        {
            countdownTimer -= Time.deltaTime;

            timerText.text = string.Format("{0:00}", countdownTimer);

            if (countdownTimer <= 0)
            {
                EndEvent();
            } 
        }
    }

    private void StartEvent()
    {
        countdownTimer = timeToCountdown;

        activeEvent = true;
        timerText.gameObject.SetActive(true);

        currentSunflowerAnimator.SetTrigger("Begin Charge");
        currentSunflowerLifebarAnimator.SetTrigger("Intro State");

    }

    private void EndEvent()
    {
        activeEvent = false;
        timerText.gameObject.SetActive(false);

        currentSunflowerLifebarAnimator.SetTrigger("Outro State");
        currentSunflowerAnimator.SetTrigger("Charge Completed");

        currentSunflower.SpawnNewTerrain();
        Destroy(currentSunflower.gameObject, 4.8f);
    }

    public void GrowSunflowerEvent(GrowthSelection growthType, Sunflower sunflower, Animator sunflowerAnimator, Animator sunflowerGrowerAinmator)
    {

        currentSunflower = sunflower;
        currentSunflowerAnimator = sunflowerAnimator;
        currentSunflowerLifebarAnimator = sunflowerGrowerAinmator;

        StartEvent();


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
