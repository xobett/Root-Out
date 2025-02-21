using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;
    bool isPaused = false;

    [SerializeField] CameraFollow cameraController;
    [SerializeField] PlayerMovement movementController;
    [SerializeField] GameObject HUD;

    private void Start()
    {
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
    }
    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        cameraController.enabled = false;
        movementController.enabled = false;
        HUD.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cameraController.enabled = true;
        movementController.enabled = true;
        HUD.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void Settings()
    {

    }
    public void VFX()
    {

    }
    public void SFX()
    {

    }
    public void Volumen()
    {

    }
}




