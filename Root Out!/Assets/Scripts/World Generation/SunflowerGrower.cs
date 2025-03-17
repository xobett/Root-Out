using UnityEngine;

public class SunflowerGrower : MonoBehaviour, IInteractable
{
    private Sunflower sunflowerToUnlock;

    [SerializeField] public bool playerIsNear;
    [SerializeField] private bool selectionActive;

    [SerializeField] private GameObject marvelousGrowthCanvas;
    [SerializeField] private GameObject genuineGrowthCanvas;
    [SerializeField] private GameObject compellingGrowthCanvas;

    [SerializeField] private Animator marvelousCanvasAnimator;
    [SerializeField] private Animator genuineCanvasAnimator;
    [SerializeField] private Animator compellingCanvasAnimator;

    private Transform mainCamera;

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        LookAtCamera();
        GrowthSelection();
    }

    public void OnInteract()
    {
        if (playerIsNear)
        {
            ActivateOptionsCanvas();
        }
    }

    private void GrowthSelection()
    {
        if (selectionActive)
        {
            if (IsPressingOne())
            {
                Debug.Log("Genuine growth was chosen.");
            }
            else if (IsPressingTwo())
            {
                Debug.Log("Marvelous growth was chosen.");
            }
            else if (IsPressingThree())
            {
                Debug.Log("Compelling growth was chosen.");
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

        DeactivateOptionsCanvas();
    }

    public void ActivateOptionsCanvas()
    {
        marvelousCanvasAnimator.SetTrigger("Start State");
        genuineCanvasAnimator.SetTrigger("Start State");
        compellingCanvasAnimator.SetTrigger("Start State");

        selectionActive = true;
    }

    private void DeactivateOptionsCanvas()
    {
        marvelousCanvasAnimator.SetTrigger("Outro State");
        genuineCanvasAnimator.SetTrigger("Outro State");
        compellingCanvasAnimator.SetTrigger("Outro State");

        selectionActive = false;
    }

    private void LookAtCamera()
    {
        Vector3 directionToFace = transform.position - mainCamera.position;
        Quaternion lookAtTarget = Quaternion.LookRotation(directionToFace, Vector3.up);
        Quaternion lookRotation = Quaternion.Euler(0, lookAtTarget.eulerAngles.y, 0);

        marvelousGrowthCanvas.transform.rotation = lookRotation;
        genuineGrowthCanvas.transform.rotation = lookRotation;
        compellingGrowthCanvas.transform.rotation = lookRotation;

    }
    private void GetReferences()
    {
        mainCamera = Camera.main.transform;
    }

    #region Input Checks

    private bool IsPressingOne() => Input.GetKeyUp(KeyCode.Alpha1);
    private bool IsPressingTwo() => Input.GetKeyUp(KeyCode.Alpha2);
    private bool IsPressingThree() => Input.GetKeyUp(KeyCode.Alpha3);

    #endregion
}
