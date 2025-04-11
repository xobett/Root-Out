using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu; // Referencia al men� de pausa
    [SerializeField] GameObject HUD; // Referencia al HUD del juego
    [SerializeField] GameObject panelResume;
    [SerializeField] GameObject panelControls; // Referencia al panel de controles
    [SerializeField] GameObject settings; // Referencia al men� de configuraci�n
    [SerializeField] GameObject mainMenu; // Referencia al men� principal
    [SerializeField] GameObject vfx; // Referencia al men� de efectos visuales
    [SerializeField] GameObject back; // Referencia al bot�n de retroceso
    [SerializeField] GameObject volumen; // Referencia al men� de volumen
    [SerializeField] GameObject ammoDisplay;
    [SerializeField] GameObject coinsDisplay;

    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject optionsPanel;

    private CameraFollow cameraController; // Referencia al controlador de la c�mara

    [SerializeField] Slider vfxVolumeSlider; // Referencia al slider de volumen de efectos visuales
    [SerializeField] Slider mainVolumeSlider; // Referencia al slider de volumen principal

    public bool isPaused = false; // Variable para controlar si el juego est� en pausa

    [SerializeField] private UIInventory uiInventory;


    private void Start()
    {
        cameraController = FindFirstObjectByType<CameraFollow>(); // Busca el controlador de la c�mara en la escena
        //pauseMenu.SetActive(false);
        //vfx.SetActive(false);
        //back.SetActive(false);
        //volumen.SetActive(false);

        // Configurar el Slider de volumen de SFX
        vfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume); // A�ade un listener para el slider de volumen de VFX
        vfxVolumeSlider.value = AudioManagerSFX.Instance.GetCurrentSFXVolume(); // Inicializa el slider con el valor actual del volumen de VFX

        // Configurar el Slider de volumen principal
        mainVolumeSlider.onValueChanged.AddListener(SetMusicClipsVolume); // A�ade un listener para el slider de volumen principal
        mainVolumeSlider.value = AudioManager.instance.GetMusicClipVolume(); // Inicializa el slider con el valor actual del volumen principal
    }

    void Update()
    {
        HandlePause();
    }

    void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !uiInventory.isOpened) // Si se presiona la tecla Escape
        {
            if (!isPaused) // Si el juego no est� en pausa
            {
                PauseGame(); // Pausa el juego
            }
            else
            {
                ResumeGame(); // Reanuda el juego
            }
        }
    }

    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None; // Desbloquea el cursor
        cameraController.enabled = false; // Desactiva el controlador de la c�mara
        GameManager.instance.gamePaused = true;
        HUD.SetActive(false);
        pauseMenu.SetActive(true); // Activa el men� de pausa
        Time.timeScale = 0f; // Detiene el tiempo del juego


        isPaused = true; // Establece la variable de pausa a true
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor
        cameraController.enabled = true; // Activa el controlador de la c�mara
        GameManager.instance.gamePaused = false;
        HUD.SetActive(true);
        pauseMenu.SetActive(false); // Desactiva el men� de pausa
        Time.timeScale = 1f; // Restaura el tiempo del juego

        isPaused = false;
    }
    public void Back()
    {
        settingsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Carga la escena del men� principal
    }
    public void Settings()
    {
        settingsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void SFX()
    {
        vfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume); // A�ade un listener para el slider de volumen de VFX
        vfxVolumeSlider.value = AudioManagerSFX.Instance.GetCurrentSFXVolume(); // Inicializa el slider con el valor actual del volumen de VFX
    }

    public void Volumen()
    {
        mainVolumeSlider.onValueChanged.AddListener(SetMusicClipsVolume); // A�ade un listener para el slider de volumen principal
        mainVolumeSlider.value = AudioManager.instance.GetMusicClipVolume(); // Inicializa el slider con el valor actual del volumen principal
    }


    // M�todo para actualizar el volumen de los SFX
    public void SetSFXVolume(float volume)
    {
        AudioManagerSFX.Instance.SetSFXVolume(volume); // Llama al m�todo del AudioManager para establecer el volumen de los VFX
    }

    // M�todo para actualizar el volumen principal
    public void SetMusicClipsVolume(float volume)
    {
        AudioManager.instance.SetMusicClipsVolume(volume); // Llama al m�todo del AudioManager para establecer el volumen principal
    }
}
    

