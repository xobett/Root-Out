using UnityEngine;

public class SunflowerGrower : MonoBehaviour, IInteractable
{
    private Sunflower sunflowerToUnlock;

    [SerializeField] private bool playerIsNear;
    [SerializeField] public bool selectionMade;
    [SerializeField] private bool selectionActive;

    [Header("GROWTH SELECTION CANVAS SETTINGS")]
    [SerializeField] private GameObject marvelousGrowthCanvas;
    [SerializeField] private GameObject genuineGrowthCanvas;
    [SerializeField] private GameObject compellingGrowthCanvas;
    [SerializeField] private GameObject sunflowerLifebarCanvas;

    [Header("GROWTH SELECTION ANIMATOR SETTINGS")]
    [SerializeField] private Animator marvelousCanvasAnimator;
    [SerializeField] private Animator genuineCanvasAnimator;
    [SerializeField] private Animator compellingCanvasAnimator;

    [Header("SUNFLOWER ANIMATOR SETTINGS")]
    [SerializeField] private Animator sunflowerLifebarAnimator;
    [SerializeField] private Animator sunflowerAnimator;

    private Transform mainCamera;

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        LookAtCamera();
        HandleGrowthSelection();
    }

    public void OnInteract()
    {
        if (playerIsNear && !selectionMade)
        {
            switch (GameManager.instance.MarvelousEventActive)
            {
                case true:
                    {
                        Debug.Log("Here is where i will send the second sunflower");

                        GameManager.instance.GetSecondSunflower(sunflowerToUnlock, sunflowerAnimator, sunflowerLifebarAnimator);
                        break;
                    }

                case false:
                    {
                        ActivateOptionsCanvas();
                        selectionActive = true;
                        break;
                    }
            }
        }
    }

    private void HandleGrowthSelection()
    {
        if (playerIsNear && !selectionMade && selectionActive)
        {
            if (IsPressingOne())
            {
                genuineCanvasAnimator.SetTrigger("Selected State");
                marvelousCanvasAnimator.SetTrigger("Outro State");
                compellingCanvasAnimator.SetTrigger("Outro State");

                GameManager.instance.GrowSunflowerEvent(GrowthSelection.Genuine, sunflowerToUnlock, sunflowerAnimator, sunflowerLifebarAnimator);
                selectionMade = true;
            }
            else if (IsPressingTwo())
            {
                marvelousCanvasAnimator.SetTrigger("Selected State");
                genuineCanvasAnimator.SetTrigger("Outro State");
                compellingCanvasAnimator.SetTrigger("Outro State");

                GameManager.instance.GrowSunflowerEvent(GrowthSelection.Marvelous, sunflowerToUnlock, sunflowerAnimator, sunflowerLifebarAnimator);
                selectionMade = true;
            }
            else if (IsPressingThree())
            {
                compellingCanvasAnimator.SetTrigger("Selected State");
                genuineCanvasAnimator.SetTrigger("Outro State");
                marvelousCanvasAnimator.SetTrigger("Outro State");

                GameManager.instance.GrowSunflowerEvent(GrowthSelection.Compelling, sunflowerToUnlock, sunflowerAnimator, sunflowerLifebarAnimator);
                selectionMade = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerIsNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerIsNear = false;
        selectionActive = false;

        DeactivateOptionsCanvas();
    }

    public void ActivateOptionsCanvas()
    {
        marvelousCanvasAnimator.SetTrigger("Start State");
        genuineCanvasAnimator.SetTrigger("Start State");
        compellingCanvasAnimator.SetTrigger("Start State");
    }

    private void DeactivateOptionsCanvas()
    {
        marvelousCanvasAnimator.SetTrigger("Outro State");
        genuineCanvasAnimator.SetTrigger("Outro State");
        compellingCanvasAnimator.SetTrigger("Outro State");
    }

    private void LookAtCamera()
    {
        Vector3 directionToFace = transform.position - mainCamera.position;
        Quaternion lookAtTarget = Quaternion.LookRotation(directionToFace, Vector3.up);
        Quaternion lookRotation = Quaternion.Euler(0, lookAtTarget.eulerAngles.y, 0);

        marvelousGrowthCanvas.transform.rotation = lookRotation;
        genuineGrowthCanvas.transform.rotation = lookRotation;
        compellingGrowthCanvas.transform.rotation = lookRotation;
        sunflowerLifebarCanvas.transform.rotation = lookRotation;

    }
    private void GetReferences()
    {
        sunflowerToUnlock = transform.parent.GetComponent<Sunflower>();
        mainCamera = Camera.main.transform;
    }

    #region Input Checks

    private bool IsPressingOne() => Input.GetKeyUp(KeyCode.Alpha1);
    private bool IsPressingTwo() => Input.GetKeyUp(KeyCode.Alpha2);
    private bool IsPressingThree() => Input.GetKeyUp(KeyCode.Alpha3);

    #endregion
}
