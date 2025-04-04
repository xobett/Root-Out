using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //Booleano que bloquea input de jugador al estar pausado el juego.
    [HideInInspector] public bool gamePaused;

    [Header("FINAL HUB SETTINGS")]
    public bool finalHubCreated;

    public int maxTerrainsPerGame = 10;
    public int totalTerrainsGenerated;

    //Handlers del jugador referenciados para acceder facilmente a ellos desde otros scripts.
    [HideInInspector] public InventoryHandler playerInventoryHandler;
    [HideInInspector] public CropHandler playerCropHandler;

    [Header("SUNFLOWER GROWTH COSTS")]
    [SerializeField] private int marvelousGrowthCost;
    [SerializeField] private int genuineGrowthCost;

    //Variables del girasol a desbloquear durante un evento.
    private Sunflower currentSunflower;
    private Animator currentSunflowerAnimator;
    private Animator currentSunflowerLifebarAnimator;

    //Variables de un segundo girasol a desbloquear durante un evento.
    private Sunflower currentSecondSunflower;
    private Animator currentSecondSunflowerAnimator;
    private Animator currentSecondSunflowerLifebarAnimator;

    [Header("ACTIVE EVENT SETTINGS")]
    public float enemySpawnTime;
    private float originalEnemySpawnTime;

    private bool marvelousEventActive;
    private bool normalEventActive;
    private bool compellingEventActive;

    private bool finalEventActive;
    public bool MarvelousEventActive => marvelousEventActive;

    [Header("MESSAGE TEXT SETTINGS")]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Animator messageTextAnimator;

    [Header("GROWTH EVENT SETTINGS")]
    [SerializeField] private TextMeshProUGUI timerText;
    private float timer;

    [SerializeField] private float compellingEventCountdown;
    [SerializeField] private float normalEventCountdown;
    [SerializeField] private float finalEventCountdown;
    
    public bool eventTimerIsActive;

    //Variables extra
    public bool explosionUpgradeActivated;

    private void Awake()
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
            else if (compellingEventActive && currentSunflower.currentHealth <= 0)
            {
                StopCoroutine(CompellingEvent());
                SetGrowthFailedAnimations();
                EndTimer();
                compellingEventActive = false;
                enemySpawnTime = originalEnemySpawnTime;

                currentSunflower.gameObject.GetComponentInChildren<SunflowerGrower>().selectionMade = false;
                
            }
        }
    }

    public void StartFinaEvent()
    {
        DisplayMessage("Survive the last minute!");
        messageTextAnimator.SetBool("secondSunflowerNotChosen", true);
        StartCoroutine(FinalEvent());
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
                        StartCoroutine(CompellingEvent());
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
    private IEnumerator FinalEvent()
    {
        finalEventActive = true;

        StartTimer();

        SpawnEnemiesInWorld();

        yield return new WaitUntil(() => timer <= 0);

        EndTimer();

        finalEventActive = false;
    }
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

        SpawnEnemiesInWorld();

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

        SpawnEnemiesInWorld();

        yield return new WaitUntil(() => timer <= 0);

        EndTimer();
        SetGrowthSuccessAnimations();

        currentSunflower.StartGrowthSuccess();
        totalTerrainsGenerated++;

        Destroy(currentSunflower.gameObject, 4.8f);

        currentSunflower = null;

        normalEventActive = false;

    }

    private IEnumerator CompellingEvent()
    {
        compellingEventActive = true;

        originalEnemySpawnTime = enemySpawnTime;
        enemySpawnTime = enemySpawnTime / 2;

        StartTimer();
        StartSunflowerAnimations();

        SpawnEnemiesInWorld();

        yield return new WaitUntil(() => timer <= 0);

        EndTimer();
        SetGrowthSuccessAnimations();

        currentSunflower.StartGrowthSuccess();
        totalTerrainsGenerated++;

        Destroy(currentSunflower.gameObject, 4.8f);

        currentSunflower = null;

        enemySpawnTime = originalEnemySpawnTime;

        compellingEventActive = false;
    }

    #endregion

    #region Game Methods

    public void PlayAgain()
    {
        var actualScene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(actualScene.buildIndex);
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion

    #region Timer Event Methods
    private void StartTimer()
    {
        timer = finalEventActive ? finalEventCountdown : compellingEventActive ? compellingEventCountdown : normalEventCountdown;
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
