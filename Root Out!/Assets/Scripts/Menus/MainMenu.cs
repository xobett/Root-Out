using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Volume globalVolume;
    Vignette vignette;

    [SerializeField] GameObject panelSettings;
    [SerializeField] GameObject panelMain;
    [SerializeField] GameObject panelLogros;

    [SerializeField] Slider mainVolumeSlider; // Referencia al slider de volumen principal
    [SerializeField] AudioSource audioSourceMain; // Referencia al AudioSource principal


    private void Start()
    {
        globalVolume = FindFirstObjectByType<Volume>();
        if (globalVolume.profile.TryGet(out Vignette vignetteComponent))
        {
            vignette = vignetteComponent;
            vignette.intensity.value = 0f; // Inicializar la intensidad de la viñeta a 0
        }

        panelSettings.SetActive(false); // Activa el menú de configuración
        panelLogros.SetActive(false);

        Cursor.lockState = CursorLockMode.None; // Desbloquear el cursor

    }

    public void Game()
    {
        // Asegurarse de que el tiempo de juego esté corriendo
        Time.timeScale = 1;

        AudioManagerSFX.Instance.PlaySFX("Entrada");
        // Iniciar la transición de la viñeta
        StartCoroutine(VignetteTransition());
    }

    private IEnumerator VignetteTransition()
    {
        float duration = 0.5f; // Duración de la transición
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float intensity = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            vignette.intensity.value = intensity;
            yield return null;
        }
        vignette.intensity.value = 1f; // Asegurarse de que la intensidad sea 1 al final

        // Cargar la escena "Game" en modo único para reiniciarla cada vez
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void Quit()
    {
        AudioManagerSFX.Instance.PlaySFX("Out");
        Application.Quit();
    }
    public void Settings()
    {
        AudioManagerSFX.Instance.PlaySFX("Into");
        panelMain.SetActive(false);
        panelSettings.SetActive(true); // Activa el menú de configuración
    }
    public void Back()
    {
        AudioManagerSFX.Instance.PlaySFX("Out");
        panelMain.SetActive(true);
        panelSettings.SetActive(false);
    }

    public void Logros()
    {
        AudioManagerSFX.Instance.PlaySFX("Into");
        panelMain.SetActive(false);
        panelLogros.SetActive(true);
    }
    public void BackLogros()
    {
        AudioManagerSFX.Instance.PlaySFX("Out");
        panelMain.SetActive(true);
        panelLogros.SetActive(false);
    }
    public void Volumen()
    {
        mainVolumeSlider.onValueChanged.AddListener(SetMainVolume); // Añade un listener para el slider de volumen principal
        mainVolumeSlider.value = audioSourceMain.volume; // Inicializa el slider con el valor actual del volumen principal
    }

    // Método para actualizar el volumen principal
    public void SetMainVolume(float volume)
    {
        audioSourceMain.volume = volume; // Establece el volumen del AudioSource principal
    }
}

