using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject panelControls;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject vfx;
    [SerializeField] GameObject back;
    //[SerializeField] GameObject sfx;
    [SerializeField] GameObject volumen;
    [SerializeField] CameraFollow cameraController;
    [SerializeField] PlayerMovement movementController;
    [SerializeField] Slider vfxVolumeSlider;
    [SerializeField] Slider mainVolumeSlider;
    [SerializeField] AudioSource audioSourceMain;

    bool isPaused = false;

    private void Start()
    {
        pauseMenu.SetActive(false);
        vfx.SetActive(false);
        back.SetActive(false);
        volumen.SetActive(false);

        // Configurar el Slider de volumen de VFX
        vfxVolumeSlider.onValueChanged.AddListener(SetVFXVolume);
        vfxVolumeSlider.value = AudioManager.instance.GetVFXVolume(); // Inicializar el Slider con el valor actual del volumen

        // Configurar el Slider de volumen principal
        mainVolumeSlider.onValueChanged.AddListener(SetMainVolume);
        mainVolumeSlider.value = audioSourceMain.volume; // Inicializar el Slider con el valor actual del volumen principal
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
        settings.SetActive(false);
        mainMenu.SetActive(false);
        panelControls.SetActive(false);
        vfx.SetActive(true);
        back.SetActive(true);
        volumen.SetActive(true);
    }

    public void VFX()
    {
        vfxVolumeSlider.onValueChanged.AddListener(SetVFXVolume);
        vfxVolumeSlider.value = AudioManager.instance.GetVFXVolume();
    }

    public void SFX()
    {
    }

    public void Volumen()
    {
        mainVolumeSlider.onValueChanged.AddListener(SetMainVolume);
        mainVolumeSlider.value = audioSourceMain.volume;
    }

    public void Back()
    {
        volumen.SetActive(false);
        vfx.SetActive(false);
        back.SetActive(false);
        mainMenu.SetActive(true);
        settings.SetActive(true);
        panelControls.SetActive(true);
    }

    // Método para actualizar el volumen de los VFX
    public void SetVFXVolume(float volume)
    {
        AudioManager.instance.SetVFXVolume(volume);
    }

    // Método para actualizar el volumen principal
    public void SetMainVolume(float volume)
    {
        audioSourceMain.volume = volume;
    }
}




