using System.Collections;
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

    [Header("PLAYER SETTINGS")]
    public InventoryHandler playerInventoryHandler;
    public CropHandler playerCropHandler;

    [Header("SUNFLOWER GROWTH COSTS")]
    [SerializeField] private int marvelousGrowthCost;
    [SerializeField] private int genuineGrowthCost;

    [Header("FIRST SUNFLOWER TO UNLOCK EVENT SETTINGS")]
    [SerializeField] private Sunflower currentSunflower;
    [SerializeField] private Animator currentSunflowerAnimator;
    [SerializeField] private Animator currentSunflowerLifebarAnimator;

    [Header("SECOND SUNFLOWER TO UNLOCK EVENT SETTINGS")]
    [SerializeField] private Sunflower currentSecondSunflower;
    [SerializeField] private Animator currentSecondSunflowerAnimator;
    [SerializeField] private Animator currentSecondSunflowerLifebarAnimator;

    [Header("MARVELOUS GROWTH EVENT SETTINGS")]
    [SerializeField] private bool marvelousEventActive;
    [SerializeField] private bool normalEventActive;
    public bool MarvelousEventActive => marvelousEventActive;

    //Change it to a method where depending on the type of event, will give a sunflower.
    public Sunflower activeSunflower => currentSunflower;

    [Header("GROWTH EVENT TIMER SETTINGS")]
    [SerializeField] private TextMeshProUGUI timerText;
    private float countdownTimer;
    private float timeToCountdown = 10f;

    //Bool usado para cerrar un evento activo.
    private bool timerIsActive;

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
        GetPlayerReferences();
    }


    private void Update()
    {
        EventTimer();
        MarvelousGrowth();
        SunflowerHealthCheck();
    }

    private void MarvelousGrowth()
    {
        if (marvelousEventActive)
        {
            Debug.Log("Choose next sunflower!");
        }
    }

    private void EventTimer()
    {
        if (timerIsActive)
        {
            countdownTimer -= Time.deltaTime;

            timerText.text = string.Format("{0:00}", countdownTimer);
        }
    }

    public void GetSecondSunflower(Sunflower secondSunflower, Animator secondSunflowerAnimator, Animator SecondSunflowerGrowerAnimator)
    {
        if (playerInventoryHandler.seedCoins >= marvelousGrowthCost)
        {
            currentSecondSunflower = secondSunflower;
            currentSecondSunflowerAnimator = secondSunflowerAnimator;
            currentSecondSunflowerLifebarAnimator = SecondSunflowerGrowerAnimator;
        }
        else
        {

        }
    }


    private void SunflowerHealthCheck()
    {
        if (marvelousEventActive && (currentSunflower.currentHealth <= 0 || currentSecondSunflower.currentHealth <= 0))
        {
            StopCoroutine(MarvelousEvent());
            SetGrowthFailedAnimations();
            marvelousEventActive = false;
        }
        else if (normalEventActive && currentSunflower.currentHealth <= 0)
        {
            StopCoroutine(NormalEvent());
            SetGrowthFailedAnimations();
            normalEventActive = false;
        }
    }

    private IEnumerator MarvelousEvent()
    {
        marvelousEventActive = true;



        //Se espera hasta que se haya elegido un segundo girasol para crecer.
        yield return new WaitUntil(() => currentSunflower != null && currentSecondSunflower != null);

        StartTimer();
        StartSunflowerAnimations();

        yield return new WaitUntil(() => countdownTimer <= 0);

        EndTimer();
        SetGrowthSuccessAnimations();

        currentSunflower.SpawnNewTerrain();
        currentSecondSunflower.SpawnNewTerrain();

        Destroy(currentSunflower.gameObject, 4.8f);
        Destroy(currentSecondSunflower.gameObject, 4.8f);

        marvelousEventActive = false;
    }

    private IEnumerator NormalEvent()
    {
        normalEventActive = true;

        StartTimer();
        StartSunflowerAnimations();

        yield return new WaitUntil(() => countdownTimer <= 0);

        EndTimer();
        SetGrowthSuccessAnimations();

        currentSunflower.SpawnNewTerrain();
        Destroy(currentSunflower.gameObject, 4.8f);

    }

    public void GrowSunflowerEvent(GrowthSelection growthType, Sunflower sunflower, Animator sunflowerAnimator, Animator sunflowerLifeBarAnimator)
    {
        currentSunflower = sunflower;
        currentSunflowerAnimator = sunflowerAnimator;
        currentSunflowerLifebarAnimator = sunflowerLifeBarAnimator;

        switch (growthType)
        {
            case GrowthSelection.Marvelous:
                {
                    if (playerInventoryHandler.seedCoins >= marvelousGrowthCost)
                    {
                        playerInventoryHandler.seedCoins -= marvelousGrowthCost;
                        StartCoroutine(MarvelousEvent());
                    }
                    else
                    {

                    }
                    break;
                }

            case GrowthSelection.Genuine:
                {
                    if (playerInventoryHandler.seedCoins >= genuineGrowthCost)
                    {
                        playerInventoryHandler.seedCoins -= genuineGrowthCost;
                        StartCoroutine(NormalEvent());
                    }
                    else
                    {

                    }
                    break;
                }

            case GrowthSelection.Compelling:
                {
                    StartCoroutine(NormalEvent());
                    break;
                }
        }
    }

    #region Timer Event Methods
    private void StartTimer()
    {
        countdownTimer = timeToCountdown;
        timerIsActive = true;
        timerText.gameObject.SetActive(true);
    }
    private void EndTimer()
    {
        countdownTimer = 0;
        timerIsActive = false;
        timerText.gameObject.SetActive(false);
    }
    #endregion

    #region Animations
    private void StartSunflowerAnimations()
    {
        currentSunflowerAnimator.SetTrigger("Begin Charge");
        currentSunflowerLifebarAnimator.SetTrigger("Intro State");

        if (marvelousEventActive)
        {
            currentSecondSunflowerAnimator.SetTrigger("Begin Charge");
            currentSecondSunflowerLifebarAnimator.SetTrigger("Intro State");
        }
    }
    private void SetGrowthSuccessAnimations()
    {
        currentSunflowerAnimator.SetTrigger("Charge Completed");
        currentSunflowerLifebarAnimator.SetTrigger("Outro State");

        if (marvelousEventActive)
        {
            currentSecondSunflowerAnimator.SetTrigger("Charge Completed");
            currentSecondSunflowerLifebarAnimator.SetTrigger("Outro State");
        }
    }
    private void SetGrowthFailedAnimations()
    {
        currentSunflowerAnimator.SetTrigger("Return to Idle");
        currentSunflowerLifebarAnimator.SetTrigger("Outro State");

        if (marvelousEventActive)
        {
            currentSecondSunflowerAnimator.SetTrigger("Return to Idle");
            currentSecondSunflowerLifebarAnimator.SetTrigger("Outro State");
        }
    }

    private void DisplayNotEnoughMoneyText()
    {

    }
    #endregion

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
    private void GetPlayerReferences()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        playerInventoryHandler = player.GetComponent<InventoryHandler>();
        playerCropHandler = player.GetComponent<CropHandler>();
    }
    #endregion
}
