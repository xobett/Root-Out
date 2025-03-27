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

    [Header("FINAL HUB SETTINGS")]
    public bool finalHubCreated;

    public int maxTerrainsPerGame = 10;
    public int totalTerrainsGenerated;

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

    [Header("ACTIVE EVENT SETTINGS")]
    [SerializeField] private bool marvelousEventActive;
    [SerializeField] private bool normalEventActive;
    [SerializeField] private bool compellingEventActive;

    [SerializeField] private bool finalEventActive;
    public bool MarvelousEventActive => marvelousEventActive;

    //Change it to a method where depending on the type of event, will give a sunflower.
    public Sunflower activeSunflower => currentSunflower;

    [Header("MESSAGE TEXT SETTINGS")]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Animator messageTextAnimator;

    [Header("GROWTH EVENT TIMER SETTINGS")]
    [SerializeField] private TextMeshProUGUI timerText;
    private float timer;

    [SerializeField] private float timeToCountdown = 10f;
    [SerializeField] private float finalEventTimer;
    
    public bool eventTimerIsActive;

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
        SunflowerHealthCheck();
    }

    private void EventTimer()
    {
        if (eventTimerIsActive)
        {
            timer -= Time.deltaTime;

            timerText.text = string.Format("{0:00}", timer);
        }
    }

    public void GetSecondSunflower(Sunflower secondSunflower, Animator secondSunflowerAnimator, Animator SecondSunflowerGrowerAnimator)
    {
        if (playerInventoryHandler.SeedCoins >= marvelousGrowthCost)
        {
            currentSecondSunflower = secondSunflower;
            currentSecondSunflowerAnimator = secondSunflowerAnimator;
            currentSecondSunflowerLifebarAnimator = SecondSunflowerGrowerAnimator;
        }
        else
        {
            StopCoroutine(MarvelousEvent());
            marvelousEventActive = false;

            messageTextAnimator.SetTrigger("Exit Sunflower selection");
            messageText.text = "You don't have enough Seeds!";
            messageTextAnimator.SetBool("secondSunflowerNotChosen", false);

            playerInventoryHandler.AddSeedCoins(marvelousGrowthCost);

            var sunflowerGrower = currentSunflower.gameObject.GetComponentInChildren<SunflowerGrower>();
            sunflowerGrower.selectionMade = false;
        }
    }


    private void SunflowerHealthCheck()
    {
        if (eventTimerIsActive)
        {
            if (marvelousEventActive && (currentSunflower.currentHealth <= 0 || currentSecondSunflower.currentHealth <= 0))
            {
                StopCoroutine(MarvelousEvent());
                SetGrowthFailedAnimations();
                EndTimer();
                marvelousEventActive = false;

                currentSunflower.gameObject.GetComponentInChildren<SunflowerGrower>().selectionMade = false;
                currentSecondSunflower.gameObject.GetComponentInChildren<SunflowerGrower>().selectionMade = false;
            }
            else if (normalEventActive && currentSunflower.currentHealth <= 0)
            {
                StopCoroutine(NormalEvent());
                SetGrowthFailedAnimations();
                EndTimer();
                normalEventActive = false;

                currentSunflower.gameObject.GetComponentInChildren<SunflowerGrower>().selectionMade = false;
            }
        }
    }

    public void StartFinaEvent()
    {
        DisplayMessage("Final event test");
        messageTextAnimator.SetBool("secondSunflowerNotChosen", true);
    }

    private IEnumerator FinalEvent()
    {
        timeToCountdown = finalEventTimer;

        StartTimer();

        yield return new WaitUntil(() => timer <= 0);

        EndTimer();
    }

    public void GrowSunflowerEvent(GrowthSelection growthType, Sunflower sunflower, Animator sunflowerAnimator, Animator sunflowerLifeBarAnimator, ref bool selectionMade)
    {
        if (totalTerrainsGenerated < maxTerrainsPerGame)
        {
            currentSunflower = sunflower;
            currentSunflowerAnimator = sunflowerAnimator;
            currentSunflowerLifebarAnimator = sunflowerLifeBarAnimator;

            switch (growthType)
            {
                case GrowthSelection.Marvelous:
                    {
                        if (playerInventoryHandler.SeedCoins >= marvelousGrowthCost)
                        {
                            playerInventoryHandler.PaySeedCoins(marvelousGrowthCost);
                            StartCoroutine(MarvelousEvent());
                        }
                        else
                        {
                            DisplayMessage("You don't have enough Seeds!");
                            selectionMade = false;
                        }
                        break;
                    }

                case GrowthSelection.Genuine:
                    {
                        if (playerInventoryHandler.SeedCoins >= genuineGrowthCost)
                        {
                            playerInventoryHandler.PaySeedCoins(genuineGrowthCost);
                            StartCoroutine(NormalEvent());
                        }
                        else
                        {
                            DisplayMessage("You don't have enough Seeds!");
                            selectionMade = false;
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
        else
        {
            DisplayMessage("You reached the limit of the map!");
            selectionMade = false;
        }
    }

    private void SpawnEnemiesInWorld()
    {
        GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("Enemy Spawner");

        foreach (GameObject enemySpawner in enemySpawners)
        {
            enemySpawner.GetComponent<EnemiesSpawner>().StartSpawner();
        }

    }

    public Sunflower GetActiveSunflower()
    {
        Sunflower sunflowerToReturn;

        if (marvelousEventActive)
        {
            Sunflower[] activeSunflowers = new Sunflower[2];
            activeSunflowers[0] = currentSunflower;
            activeSunflowers[1] = currentSunflower;

            sunflowerToReturn = activeSunflowers[Random.Range(0, 1)];
        }
        else
        {
            sunflowerToReturn = currentSunflower;
        }

        return sunflowerToReturn;
    }

    #region Event Methods

    private IEnumerator MarvelousEvent()
    {
        marvelousEventActive = true;

        DisplayMessage("Select a second Sunflower to grow!");
        messageTextAnimator.SetBool("secondSunflowerNotChosen", true);

        //Se espera hasta que se haya elegido un segundo girasol para crecer.
        yield return new WaitUntil(() => currentSunflower != null && currentSecondSunflower != null);

        messageTextAnimator.SetBool("secondSunflowerNotChosen", false);

        StartTimer();
        StartSunflowerAnimations();

        yield return new WaitUntil(() => timer <= 0);

        EndTimer();
        SetGrowthSuccessAnimations();

        currentSunflower.StartGrowthSuccess();
        totalTerrainsGenerated++;

        currentSecondSunflower.StartGrowthSuccess();
        totalTerrainsGenerated++;

        Destroy(currentSunflower.gameObject, 4.8f);
        Destroy(currentSecondSunflower.gameObject, 4.8f);

        currentSunflower = null;
        currentSecondSunflower = null;

        marvelousEventActive = false;
    }
    private IEnumerator NormalEvent()
    {
        normalEventActive = true;

        StartTimer();
        StartSunflowerAnimations();

        yield return new WaitUntil(() => timer <= 0);

        EndTimer();
        SetGrowthSuccessAnimations();

        currentSunflower.StartGrowthSuccess();
        totalTerrainsGenerated++;

        Destroy(currentSunflower.gameObject, 4.8f);

        currentSunflower = null;

        normalEventActive = false;

    }

    #endregion

    #region Timer Event Methods
    private void StartTimer()
    {
        timer = timeToCountdown;
        eventTimerIsActive = true;
        timerText.gameObject.SetActive(true);
    }
    private void EndTimer()
    {
        eventTimerIsActive = false;
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

        Debug.Log("Should set success animations");

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
    public void DisplayMessage(string message)
    {
        messageText.text = message;
        messageTextAnimator.SetTrigger("Show Message");
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
