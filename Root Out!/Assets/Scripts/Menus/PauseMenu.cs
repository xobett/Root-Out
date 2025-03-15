using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu; // Referencia al menú de pausa
    [SerializeField] GameObject HUD; // Referencia al HUD del juego
    [SerializeField] GameObject panelResume;
    [SerializeField] GameObject panelControls; // Referencia al panel de controles
    [SerializeField] GameObject settings; // Referencia al menú de configuración
    [SerializeField] GameObject mainMenu; // Referencia al menú principal
    [SerializeField] GameObject vfx; // Referencia al menú de efectos visuales
    [SerializeField] GameObject back; // Referencia al botón de retroceso
    [SerializeField] GameObject volumen; // Referencia al menú de volumen
    //[SerializeField] GameObject sfx; // Referencia a los efectos de sonido

    [SerializeField] CameraFollow cameraController; // Referencia al controlador de la cámara
    [SerializeField] PlayerMovement movementController; // Referencia al controlador de movimiento del jugador
    [SerializeField] Slider vfxVolumeSlider; // Referencia al slider de volumen de efectos visuales
    [SerializeField] Slider mainVolumeSlider; // Referencia al slider de volumen principal
    [SerializeField] AudioSource audioSourceMain; // Referencia al AudioSource principal

    bool isPaused = false; // Variable para controlar si el juego está en pausa

    private void Start()
    {
        pauseMenu.SetActive(false);
        vfx.SetActive(false);
        back.SetActive(false);
        volumen.SetActive(false);

        // Configurar el Slider de volumen de VFX
        vfxVolumeSlider.onValueChanged.AddListener(SetVFXVolume); // Añade un listener para el slider de volumen de VFX
        vfxVolumeSlider.value = AudioManager.instance.GetVFXVolume(); // Inicializa el slider con el valor actual del volumen de VFX

        // Configurar el Slider de volumen principal
        mainVolumeSlider.onValueChanged.AddListener(SetMainVolume); // Añade un listener para el slider de volumen principal
        mainVolumeSlider.value = audioSourceMain.volume; // Inicializa el slider con el valor actual del volumen principal
    }

    void Update()
    {
        HandlePause();
    }

    void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Si se presiona la tecla Escape
        {
            if (!isPaused) // Si el juego está en pausa
            {

                PauseGame(); // Pausa el juego
            }
        }
    }

    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None; // Desbloquea el cursor
        cameraController.enabled = false; // Desactiva el controlador de la cámara
        movementController.enabled = false; // Desactiva el controlador de movimiento del jugador
        HUD.SetActive(false); // Desactiva el HUD
        pauseMenu.SetActive(true); // Activa el menú de pausa
        Time.timeScale = 0f; // Detiene el tiempo del juego
        isPaused = true; // Establece la variable de pausa a true
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor
        cameraController.enabled = true; // Activa el controlador de la cámara
        movementController.enabled = true; // Activa el controlador de movimiento del jugador
        HUD.SetActive(true); // Activa el HUD
        pauseMenu.SetActive(false); // Desactiva el menú de pausa
        Time.timeScale = 1f; // Restaura el tiempo del juego
        isPaused = false; // Establece la variable de pausa a false
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Carga la escena del menú principal
    }

    public void Settings()
    {
        settings.SetActive(false); // Desactiva el menú de configuración
        mainMenu.SetActive(false); // Desactiva el menú principal
        panelControls.SetActive(false); // Desactiva el panel de controles
        panelResume.SetActive(false);
        vfx.SetActive(true); // Activa el menú de efectos visuales
        back.SetActive(true); // Activa el botón de retroceso
        volumen.SetActive(true); // Activa el menú de volumen
    }

    public void VFX()
    {
        vfxVolumeSlider.onValueChanged.AddListener(SetVFXVolume); // Añade un listener para el slider de volumen de VFX
        vfxVolumeSlider.value = AudioManager.instance.GetVFXVolume(); // Inicializa el slider con el valor actual del volumen de VFX
    }

    public void Volumen()
    {
        mainVolumeSlider.onValueChanged.AddListener(SetMainVolume); // Añade un listener para el slider de volumen principal
        mainVolumeSlider.value = audioSourceMain.volume; // Inicializa el slider con el valor actual del volumen principal
    }

    public void Back()
    {
        volumen.SetActive(false); // Desactiva el menú de volumen
        vfx.SetActive(false); // Desactiva el menú de efectos visuales
        back.SetActive(false); // Desactiva el botón de retroceso
        mainMenu.SetActive(true); // Activa el menú principal
        settings.SetActive(true); // Activa el menú de configuración
        panelControls.SetActive(true); // Activa el panel de controles
        panelResume.SetActive(true);
    }

    // Método para actualizar el volumen de los VFX
    public void SetVFXVolume(float volume)
    {
        AudioManager.instance.SetVFXVolume(volume); // Llama al método del AudioManager para establecer el volumen de los VFX
    }

    // Método para actualizar el volumen principal
    public void SetMainVolume(float volume)
    {
        audioSourceMain.volume = volume; // Establece el volumen del AudioSource principal
    }
}




