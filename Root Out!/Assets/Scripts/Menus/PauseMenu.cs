using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] 
    GameObject pauseMenu;
    bool isPaused = false;

    //CameraController cameraController;
    //MovementController movementController;

    private void Start()
    {
        //cameraController = FindAnyObjectByType<CameraController>();
        //movementController = FindAnyObjectByType<MovementController>();
        pauseMenu.SetActive(false);

    }
    void Update()
    {
        HandlePause();
    }
    void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        void PauseGame()
        {
            //cameraController.enabled = false;
            //movementController.enabled = false;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
        void ResumeGame()
        {
            //cameraController.enabled = true;
            //movementController.enabled = true;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }
}


