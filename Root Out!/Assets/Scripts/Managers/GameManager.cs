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
    }

    private void Update()
    {
        EventTimer();
        MarvelousGrowth();
    }

    private void MarvelousGrowth()
    {
        if (marvelousEventActive)
        {

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

    private void StartEvent()
    {
        countdownTimer = timeToCountdown;

        timerIsActive = true;
        timerText.gameObject.SetActive(true);

        currentSunflowerAnimator.SetTrigger("Begin Charge");
        currentSunflowerLifebarAnimator.SetTrigger("Intro State");

    }

    private void EndEvent()
    {
        timerIsActive = false;
        timerText.gameObject.SetActive(false);

        currentSunflowerLifebarAnimator.SetTrigger("Outro State");
        currentSunflowerAnimator.SetTrigger("Charge Completed");

        currentSunflower.SpawnNewTerrain();
        Destroy(currentSunflower.gameObject, 4.8f);
    }

    public void GetSecondSunflower(Sunflower secondSunflower, Animator secondSunflowerAnimator, Animator SecondSunflowerGrowerAnimator)
    {
        currentSecondSunflower = secondSunflower;
        currentSecondSunflowerAnimator = secondSunflowerAnimator;
        currentSecondSunflowerLifebarAnimator = SecondSunflowerGrowerAnimator;
    }

    private IEnumerator MarvelousEvent()
    {
        yield return new WaitUntil(() => currentSunflower != null && currentSecondSunflower != null);

        countdownTimer = timeToCountdown;

        timerIsActive = true;
        timerText.gameObject.SetActive(true);

        currentSunflowerAnimator.SetTrigger("Begin Charge");
        currentSunflowerLifebarAnimator.SetTrigger("Intro State");

        currentSecondSunflowerAnimator.SetTrigger("Begin Charge");
        currentSecondSunflowerLifebarAnimator.SetTrigger("Intro State");

        yield return new WaitUntil(() => countdownTimer <= 0);

        currentSunflowerAnimator.SetTrigger("Charge Completed");
        currentSunflowerLifebarAnimator.SetTrigger("Outro State");

        currentSecondSunflowerAnimator.SetTrigger("Charge Completed");
        currentSecondSunflowerLifebarAnimator.SetTrigger("Outro State");

        currentSunflower.SpawnNewTerrain();
        currentSecondSunflower.SpawnNewTerrain();

        Destroy(currentSunflower.gameObject, 4.8f);
        Destroy(currentSecondSunflower.gameObject, 4.8f);

        Debug.Log("Test reach");

        marvelousEventActive = false;

    }

    public void GrowSunflowerEvent(GrowthSelection growthType, Sunflower sunflower, Animator sunflowerAnimator, Animator sunflowerLifeBarAnimator)
    {
        var playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryHandler>();

        if (playerInventory.seedCoins >= 50)
        {
            currentSunflower = sunflower;
            currentSunflowerAnimator = sunflowerAnimator;
            currentSunflowerLifebarAnimator = sunflowerLifeBarAnimator;

            //StartEvent();


            switch (growthType)
            {
                case GrowthSelection.Marvelous:
                    {
                        marvelousEventActive = true;

                        StartCoroutine(MarvelousEvent());
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
        else
        {
            Debug.Log("You dont have enough coins!");
        }
    }

    private IEnumerator NormalEvent()
    {
        yield return new WaitUntil(() => currentSunflower != null);
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
